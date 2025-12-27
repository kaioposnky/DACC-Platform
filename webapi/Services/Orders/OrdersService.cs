using DaccApi.Exceptions;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Infrastructure.Mail;
using DaccApi.Infrastructure.MercadoPago.Constants;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Repositories.Reservas;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses.Order;

namespace DaccApi.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IProdutosRepository _produtosRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMailService _mailService;
        private readonly IRepositoryDapper _dapper;
        private readonly int _orderMinutesToExpire;
        
        public OrdersService(
            IOrdersRepository ordersRepository, 
            IMercadoPagoService mercadoPagoService, 
            IProdutosRepository produtosRepository,
            IReservaRepository reservaRepository,
            IConfiguration configuration,
            IRepositoryDapper dapper, IMailService mailService, IUsuarioRepository usuarioRepository)
        {
            _ordersRepository = ordersRepository;
            _mercadoPagoService = mercadoPagoService;
            _produtosRepository = produtosRepository;
            _reservaRepository = reservaRepository;
            _orderMinutesToExpire = int.Parse(configuration["OrderExpireMinutes"]);
            _dapper = dapper;
            _mailService = mailService;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CreateOrderResponse> CreateOrderWithPayment(Guid userId, CreateOrderRequest request)
        {
            var user = await _usuarioRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado!");
            }
            
            var variationIds = request.ItensPedido.Select(item => item.ProdutoVariacaoId).ToList();

            var productVariationsInfo = await _produtosRepository.GetVariationsWithProductByIdsAsync(variationIds);

            if (productVariationsInfo.Count != request.ItensPedido.Count)
            {
                throw new ArgumentException("Um ou mais produtos não foram encontrados ou estão indisponíveis!");
            }

            var orderItemsData = request.ItensPedido
                .Join(productVariationsInfo,
                    item => item.ProdutoVariacaoId,
                    produto => produto.VariationId,
                    (item, produto) => new
                    {
                        Item = item,
                        Produto = produto,
                        SubTotal = produto.Preco * item.Quantidade
                    })
                .ToList();

            var totalAmount = orderItemsData.Sum(x => x.SubTotal);
            
            var orderItems = orderItemsData.Select(data => new OrderItem
            {
                Id = Guid.NewGuid(),
                PrecoUnitario = data.Produto.Preco,
                ProdutoId = data.Produto.ProductId,
                ProdutoVariacaoId = data.Produto.VariationId,
                Quantidade = data.Item.Quantidade
            }).ToList();
            
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalAmount = totalAmount,
                OrderItems = orderItems,
                Status = MercadoPagoConstants.PaymentStatus.Pending,
                OrderDate = DateTime.UtcNow
            };

            var orderExpireDate = DateTime.Now.AddMinutes(_orderMinutesToExpire);
            
            var reservas = orderItemsData.Select(data => new ProdutoReserva()
                {
                    OrderId = order.Id, 
                    ProductVariationId = data.Produto.VariationId, 
                    ExpiresAt = orderExpireDate, 
                    Quantity = data.Item.Quantidade,
                    IsActive = true
                })
                .ToList();

            PaymentResponse preference;
            using var transaction = _dapper.BeginTransaction();
            try
            {
                var totalReservado = await _reservaRepository.CreateReservasLoteAtomica(reservas, transaction);
                var totalSolicitado = reservas.Sum(r => r.Quantity);

                if (totalReservado < totalSolicitado)
                {
                    throw new ProductOutOfStockException(
                        "Um ou mais produtos não possuem estoque suficiente no momento!");
                }

                await _ordersRepository.CreateOrder(order, transaction);
                await _ordersRepository.CreateOrderItems(order.Id, orderItems, transaction);
                
                preference = await _mercadoPagoService.CreatePreferenceAsync(
                    order, productVariationsInfo, orderExpireDate);
                order.PreferenceId = preference.PreferenceId;
            
                await _ordersRepository.UpdateOrderPreferenceId(order.Id, order.PreferenceId, transaction);
                
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            try
            {
                await _mailService.SendOrderCreatedEmailAsync(user, order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao enviar e-mail de criação de pedido: {ex.Message}");
            }

            return new CreateOrderResponse(order);
        }

        public async Task<OrderResponse> GetOrderById(Guid id)
        {
            var order = await _ordersRepository.GetOrderById(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Pedido não encontrado!");
            }

            order.OrderItems = await _ordersRepository.GetOrderItemsByOrderId(id);

            return order.ToOrderResponse();
        }

        public async Task<List<OrderResponse>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _ordersRepository.GetOrdersByUserId(userId);
            var orderResponses = new List<OrderResponse>();

            if (orders.Count == 0)
            {
                throw new KeyNotFoundException("Nenhum pedido encontrado para o usuário!");
            }
            
            foreach (var order in orders)
            {
                order.OrderItems = await _ordersRepository.GetOrderItemsByOrderId(order.Id);
                orderResponses.Add(order.ToOrderResponse());
            }

            return orderResponses;
        }

        public async Task UpdateOrderStatus(Guid id, string status)
        {
            await _ordersRepository.UpdateOrderStatus(id, status);
        }
        
        public async Task ProcessWebhookPayment(long paymentId)
        {
            try
            {
                var paymentStatus = await _mercadoPagoService.GetPaymentStatusAsync(paymentId); 
                
                var orderId = paymentStatus.ExternalReference;
                var order = await _ordersRepository.GetOrderById(orderId);
                
                if (order == null) return;
                
                switch (order.Status)
                {
                    case MercadoPagoConstants.PaymentStatus.Pending:
                        await ProcessPendingOrder(order, paymentStatus);
                        break;
                    case MercadoPagoConstants.PaymentStatus.Approved:
                        return;
                    case MercadoPagoConstants.PaymentStatus.Rejected:
                    case MercadoPagoConstants.PaymentStatus.Cancelled:
                        await CancelOrder(orderId);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception)
            {
                // Webhook retorna 200 de qualquer forma
            }
        }

        private async Task ProcessPendingOrder(Order order, PaymentStatusResponse paymentStatus)
        {
            using var transaction = _dapper.BeginTransaction();
            try
            {
                var user = await _usuarioRepository.GetByIdAsync(order.UserId);

                if (user == null)
                {
                    await CancelOrder(order.Id);
                    return;
                }
                
                await _ordersRepository.UpdateOrderPaymentInfo(
                    order.Id,
                    paymentStatus.PaymentId,
                    paymentStatus.PaymentMethod!,
                    paymentStatus.Status, 
                    transaction
                );
                
                var orderItems = await _ordersRepository.GetOrderItemsByOrderId(order.Id, transaction);
                
                var variationIds = orderItems.Select(item => item.ProdutoVariacaoId).ToList();
                var quantities = orderItems.Select(item => item.Quantidade).ToList();
                
                await _reservaRepository.ConfirmarReserva(order.Id, transaction);
                await _produtosRepository.RemoveMultipleProductsStockDirectAsync(variationIds, quantities, transaction);
                    
                transaction.Commit();
                
                await _mailService.SendOrderConfirmationEmailAsync(user, order);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        
        private async Task CancelOrder(Guid orderId)
        {
            using var transaction = _dapper.BeginTransaction();
            try
            {
                await _ordersRepository.UpdateOrderStatus(orderId,
                    MercadoPagoConstants.PaymentStatus.Rejected, transaction);
                await _reservaRepository.CancelarReserva(orderId, transaction);
                            
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
