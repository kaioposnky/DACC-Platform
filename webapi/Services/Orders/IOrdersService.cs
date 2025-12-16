using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;
using DaccApi.Model.Responses.Order;

namespace DaccApi.Services.Orders
{
    public interface IOrdersService
    {
        Task<CreateOrderResponse> CreateOrderWithPayment(Guid userId, CreateOrderRequest request);
        Task<OrderResponse> GetOrderById(Guid id);
        Task<List<OrderResponse>> GetOrdersByUserId(Guid userId);
        Task UpdateOrderStatus(Guid id, string status);
        Task ProcessWebhookPayment(long paymentId);
    }
}
