using System.Data;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Reservas
{
    /// <summary>
    /// Define a interface para o repositório de reservas de produtos.
    /// </summary>
    public interface IReservaRepository
    {
        /// <summary>
        /// Cria uma nova reserva de produto.
        /// </summary>
        Task CreateReserva(ProdutoReserva produtoReserva);
        /// <summary>
        /// Cria múltiplas reservas de produto em lote.
        /// </summary>
        Task CreateReservasLote(List<ProdutoReserva> reservas);
        /// <summary>
        /// Cria múltiplas reservas de produto em lote de forma atômica.
        /// </summary>
        Task<int> CreateReservasLoteAtomica(List<ProdutoReserva> reservas, IDbTransaction? transaction = null);
        /// <summary>
        /// Obtém todas as reservas ativas para uma variação de produto.
        /// </summary>
        Task<List<ProdutoReserva>> GetReservasAtivasByVariacao(Guid produtoVariacaoId);
        /// <summary>
        /// Obtém a quantidade total reservada para uma variação de produto.
        /// </summary>
        Task<int> GetQuantidadeReservadaByVariacao(Guid produtoVariacaoId);
        /// <summary>
        /// Confirma uma reserva de produto.
        /// </summary>
        Task ConfirmarReserva(Guid pedidoId, IDbTransaction? transaction = null);
        /// <summary>
        /// Cancela uma reserva de produto.
        /// </summary>
        Task CancelarReserva(Guid pedidoId, IDbTransaction? transaction = null);
        /// <summary>
        /// Limpa todas as reservas expiradas.
        /// </summary>
        Task LimparReservasExpiradas();
    }
}