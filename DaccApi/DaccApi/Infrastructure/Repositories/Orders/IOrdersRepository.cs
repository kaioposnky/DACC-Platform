using System.Data;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    public interface IOrdersRepository
    {
        Task<Guid> CreateOrder(Order order, IDbTransaction? transaction = null);
        Task CreateOrderItem(Guid orderId, OrderItem item, IDbTransaction? transaction = null);
        Task CreateOrderItems(Guid orderId, List<OrderItem> items, IDbTransaction? transaction = null);
        Task<Order?> GetOrderById(Guid id);
        Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId, IDbTransaction? transaction = null);
        Task<List<Order>> GetOrdersByUserId(Guid userId);
        Task UpdateOrderStatus(Guid id, string status, IDbTransaction? transaction = null);
        Task UpdateOrderPaymentInfo(Guid orderId, long paymentId, string paymentMethod, string status, IDbTransaction? transaction = null);
        Task UpdateOrderPreferenceId(Guid orderId, string preferenceId, IDbTransaction? transaction = null);
    }
}