using System.Data;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Reservas
{
    public interface IReservaRepository
    {
        Task CreateReserva(ProdutoReserva produtoReserva);
        Task CreateReservasLote(List<ProdutoReserva> reservas);
        Task<int> CreateReservasLoteAtomica(List<ProdutoReserva> reservas, IDbTransaction? transaction = null);
        Task<List<ProdutoReserva>> GetReservasAtivasByVariacao(Guid produtoVariacaoId);
        Task<int> GetQuantidadeReservadaByVariacao(Guid produtoVariacaoId);
        Task ConfirmarReserva(Guid pedidoId, IDbTransaction? transaction = null);
        Task CancelarReserva(Guid pedidoId, IDbTransaction? transaction = null);
        Task LimparReservasExpiradas();
    }
}