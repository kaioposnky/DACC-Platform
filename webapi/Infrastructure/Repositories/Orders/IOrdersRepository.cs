using System.Data;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests.Order;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    /// <summary>
    /// Define a interface para o repositório de pedidos.
    /// </summary>
    public interface IOrdersRepository
    {
        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        Task<Guid> CreateOrder(Order order, IDbTransaction? transaction = null);
        /// <summary>
        /// Cria um novo item de pedido.
        /// </summary>
        Task CreateOrderItem(Guid orderId, OrderItem item, IDbTransaction? transaction = null);
        /// <summary>
        /// Cria múltiplos itens de pedido.
        /// </summary>
        Task CreateOrderItems(Guid orderId, List<OrderItem> items, IDbTransaction? transaction = null);
        /// <summary>
        /// Obtém um pedido específico pelo seu ID.
        /// </summary>
        Task<Order?> GetOrderById(Guid id);
        /// <summary>
        /// Obtém todos os itens de um pedido específico.
        /// </summary>
        Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId, IDbTransaction? transaction = null);
        /// <summary>
        /// Obtém todos os pedidos de um usuário específico.
        /// </summary>
        Task<List<Order>> GetOrdersByUserId(Guid userId);
        /// <summary>
        /// Busca pedidos com base em filtros.
        /// </summary>
        Task<List<Order>> SearchOrdersAsync(Guid userId, RequestQueryOrders query);
        /// <summary>
        /// Atualiza o status de um pedido.
        /// </summary>
        Task UpdateOrderStatus(Guid id, string status, IDbTransaction? transaction = null);
        /// <summary>
        /// Atualiza as informações de pagamento de um pedido.
        /// </summary>
        Task UpdateOrderPaymentInfo(Guid orderId, long paymentId, string paymentMethod, string status, IDbTransaction? transaction = null);
        /// <summary>
        /// Atualiza o ID da preferência de pagamento de um pedido.
        /// </summary>
        Task UpdateOrderPreferenceId(Guid orderId, string preferenceId, IDbTransaction? transaction = null);
    }
}
