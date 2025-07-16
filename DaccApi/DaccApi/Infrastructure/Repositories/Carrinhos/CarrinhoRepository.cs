using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Carrinhos
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public CarrinhoRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<List<Carrinho>> GetUserCarrinhos(Guid usuarioId)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetUserCarrinho");
            
            var param = new { Id = usuarioId };
            
            var queryResult = await _repositoryDapper.QueryAsync<Carrinho>(sql, param);
            
            var carrinhos = queryResult.ToList();
            
            return carrinhos;
        }
    }
}