using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;

namespace DaccApi.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Guid> CreateOrderWithPayment(Guid userId, CreateOrderRequest request)
        {

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                TotalAmount = request.TotalAmount,
                OrderItems = OrderItem.FromRequestList(request.OrderItems),
            };
            
            var orderId = await _ordersRepository.CreateOrder(order);
            
            foreach (var item in request.OrderItems)
            {
                await _ordersRepository.CreateOrderItem(orderId, item);
            }

            return orderId;
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
    }
}