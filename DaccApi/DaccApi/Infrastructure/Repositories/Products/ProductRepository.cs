using System.Threading.Tasks;
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
                throw new Exception("Erro ao obter todos os produtos no banco de dados!");
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
                throw new Exception("Erro ao obter produto pelo Id no banco de dados!");
            }
        }

        public async void AddProductAsync(Produto product)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProduct");

                await _repositoryDapper.ExecuteAsync(sql, product);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar produto no banco de dados!");
            }
        }

        public async void RemoveProductByIdAsync(Guid? id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("RemoveProductById");
                var param = new { Id = id };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover produto no banco de dados!");
            }
        }

        public async void AddProductRatingAsync(ProductRating productRating)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("");

                await _repositoryDapper.ExecuteAsync(sql, productRating);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar avaliação do produto no banco de dados");
            }
        }
    }
}
