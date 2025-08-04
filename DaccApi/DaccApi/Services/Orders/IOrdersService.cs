using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;

namespace DaccApi.Services.Orders
{
    public interface IOrdersService
    {
        Task<Guid> CreateOrderWithPayment(Guid userId, CreateOrderRequest request);
        Task<OrderResponse> GetOrderById(Guid id);
        Task<List<OrderResponse>> GetOrdersByUserId(Guid userId);
        Task UpdateOrderStatus(Guid id, string status);
    }
}
