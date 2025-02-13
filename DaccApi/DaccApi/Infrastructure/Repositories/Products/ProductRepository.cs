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

        public Product GetProductByIdAsync(int id)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            var param = new { };

            // Deus por favor me ajuda a tipar essa desgrama corretamente :pray:
            return (Product)Task.Run(() =>
            {
                return _repositoryDapper.QueryAsync<Product>(sql, param)
                .GetAwaiter().GetResult();
                
            }).GetAwaiter().GetResult();
        }


        public void AddProductAsync(Product product)
        {

            var sql = _repositoryDapper.GetQueryNamed("AddProduct");
            var param = new { 
                Name = product.Name, 
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Description = product.Description,
                Id = product.Id,
                ReleaseDate = product.ReleaseDate,
            };

            Task.Run(() =>
            {
                try
                {
                    // Tenta executar a tarefa de adicionar o produto
                    _repositoryDapper.ExecuteAsync(sql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
                
            });

        }

        public void RemoveProductByIdAsync(int id)
        {

            var sql = _repositoryDapper.GetQueryNamed("RemoveProductById");
            var param = new
            { Id = id };

            Task.Run(() =>
            {
                try
                {
                    // Tenta executar a tarefa de adicionar o produto
                    _repositoryDapper.ExecuteAsync(sql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }

            });

        }

        public void AddProductRatingAsync(ProductRating productRating)
        {
            var sql = _repositoryDapper.GetQueryNamed("");
            var param = new
            {
                Rating = productRating.Rating,
                Commentary = productRating.Commentary,
                ProductId = productRating.ProductId,
                UserId = productRating.UserId,
                State = productRating.State,
                DatePosted = productRating.DatePosted,
            };

            Task.Run(() =>
            {

                try
                {
                    // Tenta executar a tarefa de adicionar o produto
                    _repositoryDapper.ExecuteAsync(sql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }

            });
        }
    }
}
