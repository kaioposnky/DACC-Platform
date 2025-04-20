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
