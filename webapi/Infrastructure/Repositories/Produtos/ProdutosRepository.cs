using System.Data;
using DaccApi.Data.Orm;
using DaccApi.Exceptions;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;
using Npgsql;

namespace DaccApi.Infrastructure.Repositories.Products
{
    /// <summary>
    /// Implementação do repositório de produtos.
    /// </summary>
    public class ProdutosRepository : BaseRepository<Produto>, IProdutosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProdutosRepository"/>.
        /// </summary>
        public ProdutosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        /// <summary>
        /// Obtém um produto específico pelo seu ID, incluindo suas variações e avaliações.
        /// </summary>
        public async Task<Produto?> GetProductByIdAsync(Guid id)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductById");
            var param = new { Id = id };
            var product = (await _repositoryDapper.QueryAsync<Produto>(sql, param)).FirstOrDefault();

            if (product == null) return null;

            product.Variacoes = await GetVariationsByProductIdAsync(id);
            product.Especificacoes = await GetProductSpecificationsAsync(id);
            product.InformacaoEnvio = await GetProductShippingInfoAsync(id);
            product.PerfeitoPara = await GetProductPerfectForAsync(id);
            product.Avaliacoes = await GetProductReviewsAsync(id);

            return product;
        }

        /// <summary>
        /// Obtém todos os produtos, incluindo suas variações.
        /// </summary>
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProducts");
            var products = (await _repositoryDapper.QueryAsync<Produto>(sql)).ToList();
            
            foreach (var product in products)
            {
                product.Variacoes = await GetVariationsByProductIdAsync(product.Id);
                product.Especificacoes = await GetProductSpecificationsAsync(product.Id);
                product.InformacaoEnvio = await GetProductShippingInfoAsync(product.Id);
                product.PerfeitoPara = await GetProductPerfectForAsync(product.Id);
            }

            return products;
        }

        /// <summary>
        /// Busca produtos com base em filtros de consulta.
        /// </summary>
        public async Task<List<Produto>> SearchProductsAsync(RequestQueryProdutos query)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("SearchProducts");

                var categoriaId = await GetCategoryIdByNameAsync(query.Category);

                var queryParams = new
                {
                    Page = query.Page,
                    Limit = query.Limit,
                    SearchPattern = string.IsNullOrWhiteSpace(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                    CategoriaID = categoriaId,
                    MinPrice = query.MinPrice,
                    MaxPrice = query.MaxPrice,
                    SortBy = query.OrderBy
                };

                var products = (await _repositoryDapper.QueryAsync<Produto>(sql, queryParams)).ToList();
                
                foreach (var product in products)
                {
                    product.Variacoes = await GetVariationsByProductIdAsync(product.Id);
                    product.Especificacoes = await GetProductSpecificationsAsync(product.Id);
                    product.InformacaoEnvio = await GetProductShippingInfoAsync(product.Id);
                    product.PerfeitoPara = await GetProductPerfectForAsync(product.Id);
                }
                
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar produtos no banco de dados." + ex.Message);
            }
        }

        public async Task<List<AvaliacaoProduto>> GetProductReviewsAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductReviews");
            var param = new { ProductId = productId };
            var reviews = await _repositoryDapper.QueryAsync<AvaliacaoProduto>(sql, param);
            return reviews.ToList();
        }

        private async Task<Guid?> GetSubcategoryIdByNameAsync(string subcategoryName)
        {
            if (string.IsNullOrEmpty(subcategoryName)) return null;
            var sql = _repositoryDapper.GetQueryNamed("GetSubcategoryIdByName");
            var param = new { Nome = subcategoryName };
            var subcategoryId = await _repositoryDapper.QueryAsync<Guid?>(sql, param);
            return subcategoryId.FirstOrDefault();
        }

        private async Task<Guid?> GetCategoryIdByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return null;
            var sql = _repositoryDapper.GetQueryNamed("GetCategoryIdByName");
            var param = new { Nome = categoryName };
            var categoryId = await _repositoryDapper.QueryAsync<Guid?>(sql, param);
            return categoryId.FirstOrDefault();
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        public async Task CreateProductAsync(Produto product)
        {
            try
            {
                var success = await CreateAsync(product);
                if (!success)
                {
                    throw new Exception("Não foi possível criar o produto.");
                }
                
                if (product.Especificacoes != null && product.Especificacoes.Any())
                {
                    await AddProductSpecificationsAsync(product.Especificacoes);
                }

                if (product.InformacaoEnvio != null)
                {
                    await AddProductShippingInfoAsync(product.InformacaoEnvio);
                }

                if (product.PerfeitoPara != null && product.PerfeitoPara.Any())
                {
                    await AddProductPerfectForAsync(product.Id, product.PerfeitoPara);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar produto no banco de dados!" + ex.Message);
            }
        }

        /// <summary>
        /// Cria uma nova variação de produto.
        /// </summary>
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
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar variação do produto no banco de dados!" + ex.Message);
            }
        }

        /// <summary>
        /// Adiciona imagens a um produto.
        /// </summary>
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

        /// <summary>
        /// Atualiza uma imagem de produto.
        /// </summary>
        public async Task UpdateProductImageAsync(ProdutoImagem imagem)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateProductImage");
                var param = new { Id = imagem.Id, ImagemUrl = imagem.ImagemUrl, Order = imagem.Ordem };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar imagem do produto no banco de dados!" + ex.Message);
            }
        }

        /// <summary>
        /// Remove um produto pelo seu ID.
        /// </summary>
        public async Task RemoveProductByIdAsync(Guid id)
        {
            try
            {
                var success = await DeleteAsync(id);

                if (!success)
                {
                    throw new Exception("Produto não encontrado ou não pôde ser removido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover produto no banco de dados!" + ex.Message);
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

        /// <summary>
        /// Obtém uma variação específica pelo seu ID.
        /// </summary>
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
                throw new Exception(
                    $"Erro ao obter variação pelo Id no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtém todas as variações de um produto.
        /// </summary>
        public async Task<List<ProdutoVariacao>> GetVariationsByProductIdAsync(Guid productId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetVariationsByProductId");
                var param = new { ProductId = productId };
                using var connection = _repositoryDapper.GetConnection();
                var variationDictionary = new Dictionary<Guid, ProdutoVariacao>();

                var variations = await connection.QueryAsync<ProdutoVariacao, ProdutoImagem, ProdutoVariacao>(
                    sql,
                    (variation, image) =>
                    {
                        if (!variationDictionary.TryGetValue(variation.Id, out var variationEntry))
                        {
                            variationEntry = variation;
                            variationEntry.Imagens = new List<ProdutoImagem>();
                            variationDictionary.Add(variationEntry.Id, variationEntry);
                        }

                        if (image != null)
                        {
                            variationEntry.Imagens.Add(image);
                        }

                        return variationEntry;
                    },
                    param,
                    splitOn: "Id"
                );

                return variations.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao obter variações do produto no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica se uma variação específica já existe.
        /// </summary>
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
                throw new Exception(
                    $"Erro ao verificar se variação existe no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Atualiza uma variação de produto existente.
        /// </summary>
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
                throw new Exception($"Erro ao atualizar variação no banco de dados. Erro original: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Deleta uma variação de produto.
        /// </summary>
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

        /// <summary>
        /// Deleta todas as imagens de uma variação.
        /// </summary>
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
                throw new Exception(
                    $"Erro ao remover imagens da variação no banco de dados. Erro original: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        public async Task UpdateProductAsync(Produto product)
        {
            try
            {
                var success = await UpdateAsync(product.Id, product);
                if (!success)
                {
                    throw new Exception("Produto não encontrado ou não pôde ser atualizado.");
                }
                
                // Atualizar especificações
                await DeleteProductSpecificationsAsync(product.Id);
                if (product.Especificacoes != null && product.Especificacoes.Any())
                {
                    await AddProductSpecificationsAsync(product.Especificacoes);
                }

                // Atualizar informações de envio
                if (product.InformacaoEnvio != null)
                {
                    await UpdateProductShippingInfoAsync(product.InformacaoEnvio);
                }

                // Atualizar ocasiões (Perfeito Para)
                await DeleteProductPerfectForAsync(product.Id);
                if (product.PerfeitoPara != null && product.PerfeitoPara.Any())
                {
                    await AddProductPerfectForAsync(product.Id, product.PerfeitoPara);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar produto no banco de dados. Erro original: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Remove uma quantidade do estoque de uma variação de produto.
        /// </summary>
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

        /// <summary>
        /// Verifica o estoque de uma variação de produto.
        /// </summary>
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

        /// <summary>
        /// Obtém informações de múltiplas variações e seus produtos.
        /// </summary>
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

        /// <summary>
        /// Verifica o estoque de múltiplos produtos.
        /// </summary>
        public async Task<bool> CheckMultipleProductsStockAsync(List<Guid> variationIds, List<int> quantities)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CheckMultipleProductsStock");

                var parameters = new
                {
                    VariationIds = variationIds.ToArray(),
                    Quantities = quantities.ToArray()
                };

                var result =
                    (await _repositoryDapper.QueryAsync<(bool success, string error_message)>(sql, parameters))
                    .FirstOrDefault();

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
                throw new Exception("Erro ao verificar estoque dos produtos.", ex);
            }
        }

        /// <summary>
        /// Remove uma quantidade do estoque de múltiplos produtos de forma atômica.
        /// </summary>
        public async Task<bool> RemoveMultipleProductsStockDirectAsync(List<Guid> variationIds,
            List<int> quantities, IDbTransaction? transaction = null)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("RemoveMultipleProductsStockDirect");

                var parameters = new
                {
                    VariationIds = variationIds.ToArray(),
                    Quantities = quantities.ToArray()
                };

                IEnumerable<(bool success, string error_message)> result;
                if (transaction != null)
                {
                    result = await _repositoryDapper.QueryAsync<(bool success, string error_message)>(sql,
                        parameters, transaction);
                }
                else
                {
                    result = await _repositoryDapper.QueryAsync<(bool success, string error_message)>(sql,
                        parameters);
                }

                var firstResult = result.FirstOrDefault();
                if (!firstResult.success)
                {
                    throw new Exception(firstResult.error_message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover estoque dos produtos.", ex);
            }
        }

        /// <summary>
        /// Obtém uma imagem específica pelo seu ID.
        /// </summary>
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
                throw new Exception($"Erro ao obter imagem pelo ID no banco de dados. Erro original: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Deleta uma imagem de produto.
        /// </summary>
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

        /// <summary>
        /// Obtém um produto a partir do ID de uma de suas variações.
        /// </summary>
        public async Task<Produto?> GetProductByProductVariationIdAsync(Guid productVariationId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetProductByProductVariationId");
                var param = new { ProductVariationId = productVariationId };
                var result = await _repositoryDapper.QueryAsync<Produto>(sql, param);
                var product = result.FirstOrDefault();
                
                if (product != null)
                {
                    product.Especificacoes = await GetProductSpecificationsAsync(product.Id);
                    product.InformacaoEnvio = await GetProductShippingInfoAsync(product.Id);
                    product.PerfeitoPara = await GetProductPerfectForAsync(product.Id);
                }
                
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao obter produto pelo id de sua variação no banco de dados. Erro original: {ex.Message}",
                    ex);
            }
        }

        private async Task AddProductSpecificationsAsync(List<ProdutoEspecificacao> specifications)
        {
            var sql = _repositoryDapper.GetQueryNamed("AddProductSpecification");
            foreach (var spec in specifications)
            {
                await _repositoryDapper.ExecuteAsync(sql, spec);
            }
        }

        private async Task DeleteProductSpecificationsAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("DeleteProductSpecifications");
            var param = new { ProdutoId = productId };
            await _repositoryDapper.ExecuteAsync(sql, param);
        }

        private async Task<List<ProdutoEspecificacao>> GetProductSpecificationsAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductSpecifications");
            var param = new { ProdutoId = productId };
            var result = await _repositoryDapper.QueryAsync<ProdutoEspecificacao>(sql, param);
            return result.ToList();
        }

        private async Task AddProductShippingInfoAsync(ProdutoInformacaoEnvio shippingInfo)
        {
            var sql = _repositoryDapper.GetQueryNamed("AddProductShippingInfo");
            await _repositoryDapper.ExecuteAsync(sql, shippingInfo);
        }

        private async Task UpdateProductShippingInfoAsync(ProdutoInformacaoEnvio shippingInfo)
        {
            var sql = _repositoryDapper.GetQueryNamed("UpdateProductShippingInfo");
            await _repositoryDapper.ExecuteAsync(sql, shippingInfo);
        }

        private async Task<ProdutoInformacaoEnvio?> GetProductShippingInfoAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductShippingInfo");
            var param = new { ProdutoId = productId };
            var result = await _repositoryDapper.QueryAsync<ProdutoInformacaoEnvio>(sql, param);
            return result.FirstOrDefault();
        }

        private async Task AddProductPerfectForAsync(Guid productId, List<string> occasions)
        {
            var sql = _repositoryDapper.GetQueryNamed("AddProductPerfectFor");
            foreach (var occasion in occasions)
            {
                var param = new { Id = Guid.NewGuid(), ProdutoId = productId, Ocasiao = occasion };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
        }

        private async Task DeleteProductPerfectForAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("DeleteProductPerfectFor");
            var param = new { ProdutoId = productId };
            await _repositoryDapper.ExecuteAsync(sql, param);
        }

        private async Task<List<string>> GetProductPerfectForAsync(Guid productId)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProductPerfectFor");
            var param = new { ProdutoId = productId };
            var result = await _repositoryDapper.QueryAsync<string>(sql, param);
            return result.ToList();
        }
    }
}
