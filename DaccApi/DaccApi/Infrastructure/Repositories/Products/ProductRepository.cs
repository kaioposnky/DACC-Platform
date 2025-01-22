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

        public Task<List<Product>> GetListProductsAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            var param = new { };

            return Task.Run(() =>
            {
                var products = _repositoryDapper.QueryAsync<Product>(sql, param).GetAwaiter().GetResult();

                var productList = new List<Product>();
                foreach (var product in products)
                {
                    productList.Add(product);
                }

                return productList;
            });
        }

    }
}
