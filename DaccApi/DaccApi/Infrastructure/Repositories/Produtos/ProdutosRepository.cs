using System.Threading.Tasks;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public ProdutosRepository(IRepositoryDapper repositoryDapper) 
        {
            _repositoryDapper = repositoryDapper;
        }
        public List<Produto> GetAllProducts()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            
            var queryResult = _repositoryDapper.Query<Produto>(sql);
                
            var products = queryResult.ToList();
                
            return products;
        }

        public Produto? GetProductById(Guid? id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetProductById");
                var param = new { Id = id };

                var queryResult = _repositoryDapper.Query<Produto>(sql, param);
                var product = queryResult.FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter produto pelo Id no banco de dados!");
            }
        }

        public async void CreateProductAsync(Produto product)
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
    }
}
