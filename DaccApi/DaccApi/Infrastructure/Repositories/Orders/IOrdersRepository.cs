using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    public interface IOrdersRepository
    {
        Task<Guid> CreateOrder(Order order);
        Task CreateOrderItem(Guid orderId, OrderItem item);
        Task CreateOrderItems(Guid orderId, List<OrderItem> items); // Novo método bulk
        Task<Order?> GetOrderById(Guid id);
        Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId);
        Task<List<Order>> GetOrdersByUserId(Guid userId);
        Task UpdateOrderStatus(Guid id, string status);
        Task UpdateOrderPaymentInfo(Guid orderId, long paymentId, string paymentMethod, string status);
    }
}