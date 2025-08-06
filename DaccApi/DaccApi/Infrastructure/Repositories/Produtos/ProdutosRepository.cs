using System.Threading.Tasks;
using DaccApi.Exceptions;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Npgsql;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public ProdutosRepository(IRepositoryDapper repositoryDapper) 
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<Produto?> GetProductByIdAsync(Guid id)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductById");
            var param = new { Id = id };
            var product = (await _repositoryDapper.QueryAsync<Produto>(sql, param)).FirstOrDefault();
            
            if (product == null) return null;
            
            product.Variacoes = await GetVariationsByProductIdAsync(id);
            
            return product;
        }
        
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            var products = (await _repositoryDapper.QueryAsync<Produto>(sql)).ToList();
            
            foreach (var product in products)
            {
                product.Variacoes = await GetVariationsByProductIdAsync(product.Id);
            }
            
            return products;
        }

        public async Task<List<Produto>> SearchProductsAsync(RequestQueryProdutos query)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("SearchProducts");
                
                var categoriaId = await GetCategoryIdByNameAsync(query.Categoria);
                var subcategoriaId = await GetSubcategoryIdByNameAsync(query.Subcategoria);
                
                var queryParams = new
                {
                    query.Page,
                    query.Limit,
                    SearchPattern = string.IsNullOrWhiteSpace(query.SearchPattern) ? null : $"%{query.SearchPattern}%",
                    CategoriaID = categoriaId,
                    SubcategoriaID = subcategoriaId,
                    query.MinPrice,
                    query.MaxPrice,
                    query.SortBy
                };
                
                var products = await _repositoryDapper.QueryAsync<Produto>(sql, queryParams);
                return products.ToList();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.MessageText);
                Console.WriteLine(ex.ColumnName);
                Console.WriteLine(ex.ConstraintName);
                Console.WriteLine(ex.Detail);
                Console.WriteLine(ex.TableName);
                throw new Exception("Erro ao buscar produtos no banco de dados." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar produtos no banco de dados." + ex.Message);
            }
        }
        
        private async Task<Guid?> GetSubcategoryIdByNameAsync(string subcategoryName)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetSubcategoryIdByName");
            var param = new { Nome = subcategoryName };
            var subcategoryId = await _repositoryDapper.QueryAsync<Guid?>(sql, param);
            return subcategoryId.FirstOrDefault();
        }
        
        private async Task<Guid?> GetCategoryIdByNameAsync(string categoryName)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetCategoryIdByName");
            var param = new { Nome = categoryName };
            var categoryId = await _repositoryDapper.QueryAsync<Guid?>(sql, param);
            return categoryId.FirstOrDefault();
        }

        public async Task CreateProductAsync(Produto product)
        {
            try
            {
                var categoryId = await GetOrCreateProductCategoryAsync(product.Categoria);
                var subCategoryId = await GetOrCreateProductSubCategoryAsync(product.Subcategoria, categoryId);
                
                var productData = new
                {
                    product.Id,
                    product.Nome,
                    product.Descricao,
                    Preco = product.Preco ?? 0,
                    PrecoOriginal = product.PrecoOriginal ?? product.Preco ?? 0,
                    SubcategoriaId = subCategoryId,
                    product.Ativo
                };
                
                var sql = _repositoryDapper.GetQueryNamed("AddProduct");
                await _repositoryDapper.QueryAsync<Guid>(sql, productData);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar produto no banco de dados!" + ex.Message);
            }
        }

        public async Task CreateProductVariationAsync(ProdutoVariacao variation)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(variation.Cor))
                    throw new Exception("Cor é obrigatória para criar variação do produto");

                if (string.IsNullOrWhiteSpace(variation.Tamanho))
                    throw new Exception("Tamanho é obrigatório para criar variação do produto");

                var colorId = await GetOrCreateColorAsync(variation.Cor);
                var sizeId = await GetOrCreateSizeAsync(variation.Tamanho);

                variation.CorId = colorId;
                variation.TamanhoId = sizeId;

                var sql = _repositoryDapper.GetQueryNamed("AddProductVariation");
                var param = new
                {
                    variation.Id,
                    variation.ProdutoId,
                    variation.CorId,
                    variation.TamanhoId,
                    variation.Estoque,
                    variation.Sku,
                    variation.Ordem,
                    variation.DataCriacao
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.MessageText);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Detail);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar variação do produto no banco de dados!" + ex.Message);
            }
        }

        public async Task AddProductImagesAsync(ProdutoImagem imagem)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProductImages");
                await _repositoryDapper.ExecuteAsync(sql, imagem);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar imagens ao produto no banco de dados!" + ex.Message); 
            }
        }
        
        public async Task UpdateProductImageAsync(ProdutoImagem imagem)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateProductImage");
                var param = new { Id = imagem.Id, ImagemUrl = imagem.ImagemUrl, Order = imagem.Ordem};
                
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar imagem do produto no banco de dados!" + ex.Message); 
            }
        }
        
        public async Task RemoveProductByIdAsync(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteProductById");
                var param = new { Id = id };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover produto no banco de dados!" + ex.Message);
            }
        }

        private async Task<Guid> GetOrCreateProductCategoryAsync(string categoriaProduto)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProductCategory");
                var param = new { Nome = categoriaProduto };

                var categoryId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                return categoryId.FirstOrDefault();
            }
            catch (PostgresException ex) when (ex.SqlState == "23505" && ex.ConstraintName == "produto_categoria_nome_key")
            {
                var sql = _repositoryDapper.GetQueryNamed("GetCategoryIdByName");
                var param = new { Nome = categoriaProduto };
                var categoryId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                return categoryId.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter/criar categoria de produto '{categoriaProduto}'", ex);
            }
        }

        private async Task<Guid> GetOrCreateProductSubCategoryAsync(string subCategoriaProduto, Guid categoryId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProductSubCategory");
                var param = new { Nome = subCategoriaProduto, CategoriaId = categoryId };

                var subcategoryId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                return subcategoryId.FirstOrDefault();
            }
            catch (PostgresException ex) when (ex.SqlState == "23505" && ex.ConstraintName == "produto_subcategoria_nome_key")
            {
                var sql = _repositoryDapper.GetQueryNamed("GetSubcategoryIdByName");
                var param = new { Nome = subCategoriaProduto };
                var subCategoryId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                return subCategoryId.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter/criar subcategoria de produto '{subCategoriaProduto}'" + ex.Message);
            }
        }

        private async Task<Guid> GetOrCreateColorAsync(string colorName)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProductColor");
                var param = new { Nome = colorName.ToLower() };
                var colorId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                var result = colorId.FirstOrDefault();
                
                if (result == Guid.Empty)
                    throw new Exception($"Falha ao criar cor '{colorName}' - ID inválido retornado");
                    
                return result;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                var sql = _repositoryDapper.GetQueryNamed("GetColorIdByName");
                var param = new { Nome = colorName.ToLower() };
                var colorId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                var result = colorId.FirstOrDefault();
                
                if (result == Guid.Empty)
                    throw new Exception($"Cor '{colorName}' existe mas não foi possível obter o ID");
                    
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter/criar cor '{colorName}': {ex.Message}", ex);
            }
        }

        private async Task<Guid> GetOrCreateSizeAsync(string sizeName)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("AddProductSize");
                var param = new { Nome = sizeName.ToUpper(), Descricao = sizeName };
                var sizeId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                var result = sizeId.FirstOrDefault();
                
                if (result == Guid.Empty)
                    throw new Exception($"Falha ao criar tamanho '{sizeName}' - ID inválido retornado");
                    
                return result;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                var sql = _repositoryDapper.GetQueryNamed("GetSizeIdByName");
                var param = new { Nome = sizeName.ToUpper() };
                var sizeId = await _repositoryDapper.QueryAsync<Guid>(sql, param);
                var result = sizeId.FirstOrDefault();
                
                if (result == Guid.Empty)
                    throw new Exception($"Tamanho '{sizeName}' existe mas não foi possível obter o ID");
                    
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter/criar tamanho '{sizeName}': {ex.Message}", ex);
            }
        }

        public async Task<ProdutoVariacao?> GetVariationByIdAsync(Guid variationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetVariationById");
                var param = new { Id = variationId };

                var queryResult = await _repositoryDapper.QueryAsync<ProdutoVariacao>(sql, param);
                var variation = queryResult.FirstOrDefault();

                if (variation == null) return variation;
                var imagesSql = _repositoryDapper.GetQueryNamed("GetVariationImages");
                var imagesParam = new { VariationId = variationId };
                var images = await _repositoryDapper.QueryAsync<ProdutoImagem>(imagesSql, imagesParam);
                variation.Imagens = images.ToList();

                return variation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter variação pelo Id no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task<List<ProdutoVariacao>> GetVariationsByProductIdAsync(Guid productId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetVariationsByProductId");
                var param = new { ProductId = productId };

                var queryResult = await _repositoryDapper.QueryAsync<ProdutoVariacao>(sql, param);
                var variations = queryResult.ToList();
                
                foreach (var variation in variations)
                {
                    var imagesSql = _repositoryDapper.GetQueryNamed("GetVariationImages");
                    var imagesParam = new { VariationId = variation.Id };
                    var images = await _repositoryDapper.QueryAsync<ProdutoImagem>(imagesSql, imagesParam);
                    variation.Imagens = images.ToList();
                }
                
                return variations;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter variações do produto no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task<bool> VariationExistsAsync(Guid productId, string cor, string tamanho)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CheckVariationExists");
                var param = new { ProductId = productId, Cor = cor, Tamanho = tamanho };

                var queryResult = await _repositoryDapper.QueryAsync<int>(sql, param);
                var count = queryResult.FirstOrDefault();
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se variação existe no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task UpdateVariationAsync(ProdutoVariacao variation)
        {
            try
            {
                // Obter ou criar cor e tamanho se necessário
                var colorId = await GetOrCreateColorAsync(variation.Cor);
                var sizeId = await GetOrCreateSizeAsync(variation.Tamanho);
                
                variation.CorId = colorId;
                variation.TamanhoId = sizeId;
                
                var sql = _repositoryDapper.GetQueryNamed("UpdateVariation");
                var param = new
                {
                    Id = variation.Id,
                    CorId = variation.CorId,
                    TamanhoId = variation.TamanhoId,
                    Estoque = variation.Estoque,
                    Ordem = variation.Ordem
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar variação no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task DeleteVariationAsync(Guid variationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteVariation");
                var param = new { Id = variationId };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover variação no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task DeleteVariationImagesAsync(Guid variationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteVariationImages");
                var param = new { VariationId = variationId };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover imagens da variação no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task UpdateProductAsync(Produto product)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateProduct");
                await _repositoryDapper.ExecuteAsync(sql, product);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar produto no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task<bool> RemoveProductVariationStockAsync(Guid productVariationId, int amount = 1)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("RemoveProductStockAmount");
                var param = new
                {
                    Id = productVariationId,
                    Amount = amount
                };
                var rowsAffected = await _repositoryDapper.ExecuteAsync(sql, param);
        
                // rowsAffected = 0 → Estoque insuficiente
                // rowsAffected = 1 → Estoque reduzido com sucesso
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover estoque de produto: {ex.Message}", ex);
            }
        }

        public async Task<int> CheckProductVariationStock(Guid productVariationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CheckProductStock");
                var param = new { Id = productVariationId };
                var result = await _repositoryDapper.QueryAsync<int>(sql, param);
                
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter estoque da variação do produto!" + ex.Message, ex);
            }
        }
        
        public async Task<List<ProdutoVariacaoInfo>> GetVariationsWithProductByIdsAsync(List<Guid> variationIds)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetVariationsWithProductByIds");
                var parameters = new { VariationIds = variationIds };
        
                var variations = await _repositoryDapper.QueryAsync<ProdutoVariacaoInfo>(sql, parameters);
                return variations.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar variações com produtos.", ex);
            }
        }
        
        public async Task<bool> RemoveMultipleProductsStockAsync(List<Guid> variationIds, List<int> quantities)
        {
            try
            {
                var parameters = new 
                { 
                    VariationIds = variationIds.ToArray(),
                    Quantities = quantities.ToArray()
                };
        
                var result = (await _repositoryDapper.
                    QueryProcedureAsync<(bool success, string error_message)>
                        ("remove_multiple_products_stock", parameters)).FirstOrDefault();
        
                if (!result.success)
                {
                    throw new ProductOutOfStockException(result.error_message);
                }
        
                return true;
            }
            catch (ProductOutOfStockException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover estoque dos produtos.", ex);
            }
        }

        public async Task<ProdutoImagem?> GetImageByIdAsync(Guid imageId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetImageById");
                var param = new { Id = imageId };
                var result = await _repositoryDapper.QueryAsync<ProdutoImagem>(sql, param);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter imagem pelo ID no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task DeleteImageAsync(Guid imageId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteImageById");
                var param = new { Id = imageId };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar imagem no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        public async Task<Produto?> GetProductByProductVariationIdAsync(Guid productVariationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetProductByProductVariationId");
                var param = new { ProductVariationId = productVariationId };
                var result = await _repositoryDapper.QueryAsync<Produto>(sql, param);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter produto pelo id de sua variação no banco de dados. Erro original: {ex.Message}", ex);
            }
        }
    }
}