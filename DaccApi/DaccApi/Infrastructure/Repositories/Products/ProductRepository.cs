using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public ProductRepository(IRepositoryDapper repositoryDapper) 
        {
            _repositoryDapper = repositoryDapper;
        }
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");

                var queryResult = await _repositoryDapper.QueryProcedureAsync<Produto>(sql);

                var products = queryResult.ToList();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os produtos na base de dados!");
            }
            
        }

        public async Task<Produto?> GetProductByIdAsync(Guid? id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetProductById");
                var param = new { Id = id };

                var queryResult = await _repositoryDapper.QueryProcedureAsync<Produto>(sql, param);
                var product = queryResult.FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter produto pelo Id na base de dados!");
            }

        }

    }
}
