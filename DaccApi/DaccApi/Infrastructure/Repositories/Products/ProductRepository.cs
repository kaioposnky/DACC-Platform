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
        public List<Product> GetListProducts()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            var param = new { };

            var products = _repositoryDapper.Query<Product>(sql).ToList();

            if (products == null || !products.Any())
            {
                Console.WriteLine("Nenhum produto encontrado.");
            }

            return products;
        }


    }
}
