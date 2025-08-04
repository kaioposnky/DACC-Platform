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
                var variationIds = request.OrderItems.Select(item => item.ProductVariationId).ToList();
                var quantities = request.OrderItems.Select(item => item.Quantity).ToList();


                // Se não tiver produtos em estoque o bastante da throw em ProductOutOfStockException
                var stockRemoved = await _produtosRepository.RemoveMultipleProductsStockAsync(variationIds, quantities);



                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TotalAmount = request.TotalAmount,
                    OrderItems = OrderItem.FromRequestList(request.OrderItems),
                    OrderDate = DateTime.UtcNow
                };

                var preference = await _mercadoPagoService.CreatePreferenceAsync(order);

                order.PreferenceId = preference.PreferenceId;

                // Cria o pedido no banco
                await _ordersRepository.CreateOrder(order);

                foreach (var item in request.OrderItems)
                {
                    await _ordersRepository.CreateOrderItem(order.Id, item);
                }

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
            catch (ProductOutOfStockException ex)
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