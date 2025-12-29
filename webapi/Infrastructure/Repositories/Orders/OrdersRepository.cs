using System.Data;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests.Order;
using Npgsql;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    /// <summary>
    /// Implementação do repositório de pedidos.
    /// </summary>
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="OrdersRepository"/>.
        /// </summary>
        public OrdersRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        public async Task<Guid> CreateOrder(Order order, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateOrder");
                var parameters = new 
                { 
                    Id = order.Id,
                    UserId = order.UserId, 
                    OrderDate = order.OrderDate, 
                    Status = "created", 
                    TotalAmount = order.TotalAmount,
                    MercadoPagoPaymentId = (long?)null, // Será definido mais tarde
                    PaymentMethod = (string?)null, // Será definido mais tarde
                    CupomId = order.CupomId
                };
                
                IEnumerable<Guid> orderId;
                if (transaction != null)
                {
                    orderId = await _repositoryDapper.QueryAsync<Guid>(sql, parameters, transaction);
                }
                else
                {
                    orderId = await _repositoryDapper.QueryAsync<Guid>(sql, parameters);
                }
                
                return orderId.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar pedido no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Cria um novo item de pedido.
        /// </summary>
        public async Task CreateOrderItem(Guid orderId, OrderItem item, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateOrderItem");
                var parameters = new
                {
                    OrderId = orderId, 
                    ProductId = item.ProdutoId, 
                    ProductVariationId = item.ProdutoVariacaoId, 
                    Quantity = item.Quantidade, 
                    UnitPrice = item.PrecoUnitario
                };
                
                if (transaction != null)
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters, transaction);
                }
                else
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar item de pedido no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Cria múltiplos itens de pedido.
        /// </summary>
        public async Task CreateOrderItems(Guid orderId, List<OrderItem> items, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateOrderItems");
                var parameters = items.Select(item => new
                {
                    OrderId = orderId,
                    ProductId = item.ProdutoId,
                    ProductVariationId = item.ProdutoVariacaoId,
                    Quantity = item.Quantidade,
                    UnitPrice = item.PrecoUnitario
                }).ToArray();

                if (transaction != null)
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters, transaction);
                }
                else
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters);
                }
            }
            catch (PostgresException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar itens de pedido no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Obtém um pedido específico pelo seu ID.
        /// </summary>
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

        /// <summary>
        /// Obtém todos os itens de um pedido específico.
        /// </summary>
        public async Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetOrderItemsByOrderId");
                var parameters = new { OrderId = orderId };
                
                IEnumerable<OrderItem> orderItems;
                if (transaction != null)
                {
                    orderItems = await _repositoryDapper.QueryAsync<OrderItem>(sql, parameters, transaction);
                }
                else
                {
                    orderItems = await _repositoryDapper.QueryAsync<OrderItem>(sql, parameters);
                }
                
                return orderItems.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter itens de pedido por ID de pedido no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Obtém todos os pedidos de um usuário específico.
        /// </summary>
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

        public async Task<List<Order>> SearchOrdersAsync(Guid userId, RequestQueryOrders query)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("SearchOrders");
                var param = new
                {
                    UserId = userId,
                    Status = query.Status,
                    StartDate = query.StartDate,
                    EndDate = query.EndDate,
                    SearchQuery = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                    Page = query.Page,
                    Limit = query.Limit
                };

                var result = await _repositoryDapper.QueryAsync<Order>(sql, param);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar pedidos no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Atualiza o status de um pedido.
        /// </summary>
        public async Task UpdateOrderStatus(Guid id, string status, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateOrderStatus");
                var parameters = new { Id = id, Status = status };
                
                if (transaction != null)
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters, transaction);
                }
                else
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar status do pedido no banco de dados.", ex);
            }
        }

        /// <summary>
        /// Atualiza as informações de pagamento de um pedido.
        /// </summary>
        public async Task UpdateOrderPaymentInfo(Guid orderId, long paymentId, string paymentMethod, string status, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateOrderPaymentInfo");
                var parameters = new 
                { 
                    Id = orderId,
                    MercadoPagoPaymentId = paymentId,
                    PaymentMethod = paymentMethod,
                    Status = status
                };
                
                if (transaction != null)
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters, transaction);
                }
                else
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar informações de pagamento do pedido.", ex);
            }
        }
        
        /// <summary>
        /// Atualiza o ID da preferência de pagamento de um pedido.
        /// </summary>
        public async Task UpdateOrderPreferenceId(Guid orderId, string preferenceId, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateOrderPreferenceId");
                var parameters = new 
                { 
                    Id = orderId,
                    PreferenceId = preferenceId
                };
                
                if (transaction != null)
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters, transaction);
                }
                else
                {
                    await _repositoryDapper.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar preferência da order! do pedido.", ex);
            }
        }
    }
}
