using DaccApi.Model.Requests;
using DaccApi.Model.Requests.Order;
using DaccApi.Model.Responses.Order;

namespace DaccApi.Services.Orders
{
    public interface IOrdersService
    {
        Task<CreateOrderResponse> CreateOrderWithPayment(Guid userId, CreateOrderRequest request);
        Task<OrderResponse> GetOrderById(Guid id);
        Task<List<OrderResponse>> GetOrdersByUserId(Guid userId);
        Task<List<OrderResponse>> SearchOrders(Guid userId, RequestQueryOrders query);
        Task UpdateOrderStatus(Guid id, string status);
        Task ProcessWebhookPayment(long paymentId);
    }
}
