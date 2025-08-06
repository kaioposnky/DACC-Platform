using DaccApi.Exceptions;
using DaccApi.Infrastructure.MercadoPago.Constants;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;

namespace DaccApi.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IProdutosRepository _produtosRepository;

        public OrdersService(IOrdersRepository ordersRepository, IMercadoPagoService mercadoPagoService, IProdutosRepository produtosRepository)
        {
            _ordersRepository = ordersRepository;
            _mercadoPagoService = mercadoPagoService;
            _produtosRepository = produtosRepository;
        }

        public async Task<CreateOrderResponse> CreateOrderWithPayment(Guid userId, CreateOrderRequest request)
        {
            try
            {
                var variationIds = request.OrderItems.Select(item => item.ProdutoVariacaoId).ToList();
                var quantities = request.OrderItems.Select(item => item.Quantidade).ToList();

                var productVariationInfo = await _produtosRepository.GetVariationsWithProductByIdsAsync(variationIds);

                if (productVariationInfo.Count != request.OrderItems.Count)
                {
                    throw new ArgumentException("Um ou mais produtos estão indisponíveis!");
                }

                // Se não tiver produtos em estoque o bastante da throw em ProductOutOfStockException
                await _produtosRepository.RemoveMultipleProductsStockAsync(variationIds, quantities);

                var productDict = productVariationInfo.ToDictionary(p => p.VariationId, p => p);
                ;

                // Soma todos o preço de todos os produtos junto com a quantidade para calcular o preço total
                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in request.OrderItems)
                {
                    if (!productDict.TryGetValue(item.ProdutoVariacaoId, out var product))
                    {
                        throw new ArgumentException($"Produto {item.ProdutoVariacaoId} não encontrado!");
                    }

                    var unitPrice = product.Preco;
                    totalAmount += unitPrice * item.Quantidade;

                    orderItems.Add(new OrderItem
                    {
                        PrecoUnitario = unitPrice,
                        ProdutoId = product.ProductId,
                        ProdutoVariacaoId = product.VariationId,
                        Quantidade = item.Quantidade
                    });
                }

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TotalAmount = totalAmount,
                    OrderItems = OrderItem.FromRequestList(request.OrderItems),
                    OrderDate = DateTime.UtcNow
                };

                var preference = await _mercadoPagoService.CreatePreferenceAsync(order);

                order.PreferenceId = preference.PreferenceId;

                // Cria o pedido no banco
                await _ordersRepository.CreateOrder(order);

                // Cria os itens do pedido no banco
                await _ordersRepository.CreateOrderItems(order.Id, orderItems);

                return new CreateOrderResponse
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    PaymentUrl = preference.PaymentUrl,
                    OrderItems = order.OrderItems,
                    Status = MercadoPagoConstants.PaymentStatus.Pending,
                    TotalAmount = order.TotalAmount,
                    OrderDate = order.OrderDate
                };
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (ProductOutOfStockException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar pedido! " + ex.Message, ex);
            }
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

                // O externalreference salvo no pagamento é o orderId
                var orderId = paymentStatus.ExternalReference;

                // Atualiza os dados do pedido com as informações do pagamento recebidas
                await _ordersRepository.UpdateOrderPaymentInfo(
                    orderId,
                    paymentStatus.PaymentId,
                    paymentStatus.PaymentMethod!,
                    paymentStatus.Status
                );
            }
            catch (Exception ex)
            {
                // engole o choro e faz o L
            }
        }
    }
}