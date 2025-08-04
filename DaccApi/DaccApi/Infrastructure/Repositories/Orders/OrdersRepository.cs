using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public OrdersRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<Guid> CreateOrder(Order order)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateOrder");
                var parameters = new 
                { 
                    Id = order.Id,
                    UserId = order.UserId, 
                    OrderDate = DateTime.UtcNow, 
                    Status = "created", 
                    TotalAmount = order.TotalAmount,
                    MercadoPagoPaymentId = order.MercadoPagoPaymentId,
                    PreferenceId = order.PreferenceId,  
                    PaymentMethod = (string?)null // Será definido mais tarde 
                };
                
                var orderId = await _repositoryDapper.QueryAsync<Guid>(sql, parameters);
                return orderId.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar pedido no banco de dados.", ex);
            }
        }

        public async Task CreateOrderItem(Guid orderId, OrderItemRequest item)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateOrderItem");
                var parameters = new
                {
                    OrderId = orderId, 
                    ProductId = item.ProductId, 
                    Quantity = item.Quantity, 
                    UnitPrice = item.UnitPrice
                };
                await _repositoryDapper.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar item de pedido no banco de dados.", ex);
            }
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetOrderById");
                var parameters = new { Id = id };
                var order = (await _repositoryDapper.QueryAsync<Order>(sql, parameters)).FirstOrDefault();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter pedido por ID no banco de dados.", ex);
            }
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetOrderItemsByOrderId");
                var parameters = new { OrderId = orderId };
                var orderItems = (await _repositoryDapper.QueryAsync<OrderItem>(sql, parameters)).ToList();
                return orderItems;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter itens de pedido por ID de pedido no banco de dados.", ex);
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(Guid userId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetOrdersByUserId");
                var parameters = new { UserId = userId };
                var orders = (await _repositoryDapper.QueryAsync<Order>(sql, parameters)).ToList();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter pedidos por ID de usuário no banco de dados.", ex);
            }
        }

        public async Task UpdateOrderStatus(Guid id, string status)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateOrderStatus");
                var parameters = new { Id = id, Status = status };
                await _repositoryDapper.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar status do pedido no banco de dados.", ex);
            }
        }

        public async Task UpdateOrderPaymentInfo(Guid orderId, long paymentId, Guid preferenceId, string paymentMethod, string status)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateOrderPaymentInfo");
                var parameters = new 
                { 
                    Id = orderId,
                    MercadoPagoPaymentId = paymentId,
                    PreferenceId = preferenceId,
                    PaymentMethod = paymentMethod,
                    Status = status
                };
                await _repositoryDapper.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar informações de pagamento do pedido.", ex);
            }
        }
    }
}