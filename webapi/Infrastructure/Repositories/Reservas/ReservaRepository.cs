using System.Data;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Reservas
{
    /// <summary>
    /// Implementação do repositório de reservas de produtos.
    /// </summary>
    public class ReservaRepository : IReservaRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReservaRepository"/>.
        /// </summary>
        public ReservaRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        /// <summary>
        /// Cria uma nova reserva de produto.
        /// </summary>
        public async Task CreateReserva(ProdutoReserva produtoReserva)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateReserva");
                var parameters = new
                {
                    ProdutoVariacaoId = produtoReserva.ProductVariationId,
                    PedidoId = produtoReserva.OrderId,
                    Quantidade = produtoReserva.Quantity,
                    Ativo = produtoReserva.IsActive,
                    DataExpira = produtoReserva.ExpiresAt
                };
                await _repositoryDapper.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar reserva: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as reservas ativas para uma variação de produto.
        /// </summary>
        public async Task<List<ProdutoReserva>> GetReservasAtivasByVariacao(Guid produtoVariacaoId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetReservasAtivasByVariacao");
                var result = await _repositoryDapper.QueryAsync<dynamic>(sql, new { ProdutoVariacaoId = produtoVariacaoId });
                
                return result.Select(r => new ProdutoReserva
                {
                    Id = r.id,
                    ProductVariationId = r.produto_variacao_id,
                    OrderId = r.pedido_id,
                    Quantity = r.quantidade,
                    ExpiresAt = r.data_expira,
                    IsActive = r.ativo
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar reservas ativas: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtém a quantidade total reservada para uma variação de produto.
        /// </summary>
        public async Task<int> GetQuantidadeReservadaByVariacao(Guid produtoVariacaoId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetQuantidadeReservadaByVariacao");
                var result = await _repositoryDapper.QueryAsync<int?>(sql, new { ProdutoVariacaoId = produtoVariacaoId });
                return result.FirstOrDefault() ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar quantidade reservada: " + ex.Message);
            }
        }

        /// <summary>
        /// Confirma uma reserva de produto.
        /// </summary>
        public async Task ConfirmarReserva(Guid pedidoId, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("ConfirmarReserva");
                var parameters = new { PedidoId = pedidoId };
                
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
                throw new Exception("Erro ao confirmar reserva: " + ex.Message);
            }
        }

        /// <summary>
        /// Cancela uma reserva de produto.
        /// </summary>
        public async Task CancelarReserva(Guid pedidoId, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CancelarReserva");
                var parameters = new { PedidoId = pedidoId };
                
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
                throw new Exception("Erro ao cancelar reserva: " + ex.Message);
            }
        }

        /// <summary>
        /// Cria múltiplas reservas de produto em lote.
        /// </summary>
        public async Task CreateReservasLote(List<ProdutoReserva> reservas)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateReservasLote");
                var parameters = reservas.Select(r => new
                {
                    ProdutoVariacaoId = r.ProductVariationId,
                    PedidoId = r.OrderId,
                    Quantidade = r.Quantity,
                    Ativo = r.IsActive,
                    DataExpira = r.ExpiresAt
                }).ToArray();
                
                await _repositoryDapper.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar reservas em lote: " + ex.Message);
            }
        }

        /// <summary>
        /// Cria múltiplas reservas de produto em lote de forma atômica.
        /// </summary>
        public async Task<int> CreateReservasLoteAtomica(List<ProdutoReserva> reservas, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateReservasLoteAtomica");
                var parameters = reservas.Select(r => new
                {
                    ProdutoVariacaoId = r.ProductVariationId,
                    PedidoId = r.OrderId,
                    Quantidade = r.Quantity,
                    Ativo = r.IsActive,
                    DataExpira = r.ExpiresAt
                }).ToArray();
                
                IEnumerable<int> result;
                if (transaction != null)
                {
                    result = await _repositoryDapper.QueryAsync<int>(sql, parameters, transaction);
                }
                else
                {
                    result = await _repositoryDapper.QueryAsync<int>(sql, parameters);
                }
                
                return result.Sum(); // Soma todas as quantidades reservadas com sucesso
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar reservas atômicas: " + ex.Message);
            }
        }

        /// <summary>
        /// Limpa todas as reservas expiradas.
        /// </summary>
        public async Task LimparReservasExpiradas()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("LimparReservasExpiradas");
                await _repositoryDapper.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao limpar reservas expiradas: " + ex.Message);
            }
        }
    }
}