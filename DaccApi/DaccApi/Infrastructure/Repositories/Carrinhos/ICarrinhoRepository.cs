using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Carrinhos
{
    public interface ICarrinhoRepository
    {
        Task<List<Carrinho?>> GetUserCarrinhos(Guid usuarioId);
    }
}