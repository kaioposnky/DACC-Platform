using DaccApi.Data.Orm.Subcategoria;
using DaccApi.Model;
using DaccApi.Model.Responses;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Responses;

namespace DaccApi.Services.Products
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ISubcategoriaRepository _subcategoriaRepository;
        private readonly IRepositoryDapper _repositoryDapper;

        public ProdutosService(IProdutosRepository produtosRepository, IFileStorageService fileStorageService, ISubcategoriaRepository subcategoriaRepository, IRepositoryDapper repositoryDapper)
        {
            _produtosRepository = produtosRepository;
            _fileStorageService = fileStorageService;
            _subcategoriaRepository = subcategoriaRepository;
            _repositoryDapper = repositoryDapper;
        }

        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _produtosRepository.GetAllProductsAsync();

                if (products.Count == 0)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT,
                        "Nenhum produto foi encontrado!");
                }

                var response = products.Select(p => Produto.MapToResponseProduto(p, p.Variacoes)).ToList();

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { products = response }), 
                    "Produtos obtidos com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao obter os produtos: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetProductByIdAsync(Guid produtoId)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(produtoId);

                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, 
                        "Nenhum produto foi encontrado com esse id!");
                }

                var response = Produto.MapToResponseProduto(product, product.Variacoes);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { product = response }), 
                    "Produto obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao obter produto pelo ID: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateProductAsync(RequestCreateProduto requestCreateProduto)
        {
            try
            {
                var productId = Guid.NewGuid();
                var categoryId = await ResolveCategoryIdAsync(requestCreateProduto.Category);
                var subcategoryId = await ResolveSubcategoryIdAsync(requestCreateProduto.Subcategory);

                var product = await CreateProductEntityAsync(requestCreateProduto, productId, categoryId, subcategoryId);

                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(new { id = product.Id }),
                    "Produto criado com sucesso! Use o endpoint de variações para adicionar opções de compra.");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.VALIDATION_ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao criar produto: {ex.Message}");
            }
        }

        private async Task<Produto> CreateProductEntityAsync(RequestCreateProduto request, Guid productId, Guid categoryId, Guid? subcategoryId)
        {
            var product = Produto.FromRequest(request, productId, categoryId, subcategoryId);
            await _produtosRepository.CreateProductAsync(product);
            return product;
        }

        public async Task<IActionResult> RemoveProductByIdAsync(Guid produtoId)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(produtoId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }
                
                await _produtosRepository.RemoveProductByIdAsync(produtoId);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Produto removido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao remover produto: {ex.Message}");
            }
        }

        public async Task<IActionResult> SearchProductsAsync(RequestQueryProdutos requestQueryProdutos)
        {
            try
            {
                var (products, totalCount) = await _produtosRepository.SearchProductsAsync(requestQueryProdutos);

                if (products.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT,
                        "Nenhum produto encontrado com os critérios de busca!");
                var response = products.Select(produto => new ResponseProduto(produto));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { products = response, totalCount = totalCount }),
                    "Produtos encontrados com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao buscar produtos: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateVariationAsync(Guid productId, RequestProdutoVariacaoCreate request)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }

                var variationExists = await _produtosRepository.VariationExistsAsync(productId, request.Color.Trim(), request.Size);
                if (variationExists)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_ALREADY_EXISTS,
                        $"Já existe uma variação com cor '{request.Color}' e tamanho '{request.Size}' para este produto!");
                }

                var variationId = Guid.NewGuid();
                var sku = ProdutoVariacao.GenerateSku(productId, request.Color.Trim(), request.Size);
                var variation = ProdutoVariacao.FromRequest(request, productId, variationId, sku);

                await _produtosRepository.CreateProductVariationAsync(variation);

                var createdVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                var response = Produto.MapToResponseVariacao(createdVariation ?? variation);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(response),
                    "Variação criada com sucesso! Use o endpoint de imagens para adicionar fotos.");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.VALIDATION_ERROR, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao criar variação: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetVariationsAsync(Guid productId)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }

                var variations = await _produtosRepository.GetVariationsByProductIdAsync(productId);
                
                var responseVariations = variations.Select(Produto.MapToResponseVariacao).ToList();

                var message = variations.Count == 0 
                    ? "Este produto ainda não possui variações. Use o endpoint de criação de variações para adicionar opções de compra."
                    : $"Encontradas {variations.Count} variações para o produto!";

                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.OK.WithData(new { variations = responseVariations }),
                    message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao obter variações: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateVariationAsync(Guid productId, Guid variationId, RequestUpdateProdutoVariacao request)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }

                var existingVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                if (existingVariation == null || existingVariation.ProdutoId != productId)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Variação não encontrada para este produto!");
                }

                if ((request.Color != null && request.Color.Trim() != existingVariation.Cor) || 
                    (request.Size != null && request.Size != existingVariation.Tamanho))
                {
                    var newCor = request.Color?.Trim() ?? existingVariation.Cor;
                    var newTamanho = request.Size ?? existingVariation.Tamanho;
                    
                    var variationExists = await _produtosRepository.VariationExistsAsync(productId, newCor.Trim(), newTamanho);
                    if (variationExists)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_ALREADY_EXISTS,
                            $"Já existe uma variação com cor '{newCor}' e tamanho '{newTamanho}' para este produto!");
                    }
                }

                existingVariation.UpdateFromRequest(request);
                
                if (request.Color != null || request.Size != null)
                {
                    existingVariation.Sku = ProdutoVariacao.GenerateSku(productId, existingVariation.Cor, existingVariation.Tamanho);
                }

                existingVariation.DataAtualizacao = DateTime.UtcNow;

                await _produtosRepository.UpdateVariationAsync(existingVariation);

                var updatedVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                var response = Produto.MapToResponseVariacao(updatedVariation ?? existingVariation);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.OK.WithData(response),
                    "Variação atualizada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.VALIDATION_ERROR, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao atualizar variação: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteVariationAsync(Guid productId, Guid variationId)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }

                var existingVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                if (existingVariation == null || existingVariation.ProdutoId != productId)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Variação não encontrada para este produto!");
                }

                try
                {
                    await _produtosRepository.DeleteVariationImagesAsync(variationId);
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao deletar variação de produto!");
                }
                
                await _produtosRepository.DeleteVariationAsync(variationId);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK,
                    "Variação removida com sucesso!");
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao remover variação: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateProductAsync(Guid productId, RequestUpdateProduto requestUpdateProduto)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);

                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Produto não encontrado!");
                }

                Guid? categoryId = null;
                if (!string.IsNullOrEmpty(requestUpdateProduto.Category))
                {
                    categoryId = await ResolveCategoryIdAsync(requestUpdateProduto.Category);
                }
                
                Guid? subcategoryId = null;
                if (requestUpdateProduto.Subcategory != null) // != null pois pode ser string vazia para limpar
                {
                    subcategoryId = await ResolveSubcategoryIdAsync(requestUpdateProduto.Subcategory);
                }

                product.UpdateFromRequest(requestUpdateProduto, categoryId, subcategoryId);

                await _produtosRepository.UpdateProductAsync(product);

                var updatedProduct = await _produtosRepository.GetProductByIdAsync(productId);
                var response = new ResponseProduto(updatedProduct);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(response), "Produto atualizado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.VALIDATION_ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, $"Erro ao atualizar produto: {ex.Message}");
            }
        }
        
        // Métodos para imagem (sem alteração)
        public async Task<IActionResult> CreateVariationImageAsync(Guid productId, Guid variationId,
            RequestCreateProdutoImagem request)
        {
            try
            {
                var product = await _produtosRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Produto não encontrado!");
                }

                var variation = await _produtosRepository.GetVariationByIdAsync(variationId);
                if (variation == null || variation.ProdutoId != productId)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Variação não encontrada para este produto!");
                }

                string imageUrl;
                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        imageUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        imageUrl = request.ImageUrl;
                    }
                }
                else
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "A imagem é obrigatória.");
                }

                var produtoImagem = new ProdutoImagem
                {
                    Id = Guid.NewGuid(),
                    ProdutoVariacaoId = variationId,
                    ImagemUrl = imageUrl,
                    ImagemAlt = request.ImageAlt?.Trim(),
                    Ordem = request.Order,
                };

                await _produtosRepository.AddProductImagesAsync(produtoImagem);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(new { imageId = produtoImagem.Id, imageUrl }),
                    "Imagem adicionada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao adicionar imagem: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetImageAsync(Guid imageId)
        {
            try
            {
                var image = await _produtosRepository.GetImageByIdAsync(imageId);
                if (image == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Imagem não encontrada!");
                }

                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.OK.WithData(new { image }),
                    "Imagem obtida com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao obter imagem: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateImageAsync(Guid imageId, RequestUpdateProdutoImagem request)
        {
            try
            {
                var existingImage = await _produtosRepository.GetImageByIdAsync(imageId);
                if (existingImage == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Imagem não encontrada!");
                }

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        existingImage.ImagemUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        existingImage.ImagemUrl = request.ImageUrl;
                    }
                }

                if (request.Order.HasValue)
                {
                    existingImage.Ordem = request.Order.Value;
                }

                if (request.ImageAlt != null)
                {
                    existingImage.ImagemAlt = request.ImageAlt.Trim();
                }

                await _produtosRepository.UpdateProductImageAsync(existingImage);

                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.OK.WithData(new { image = existingImage }),
                    "Imagem atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao atualizar imagem: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteImageAsync(Guid imageId)
        {
            try
            {
                var existingImage = await _produtosRepository.GetImageByIdAsync(imageId);
                if (existingImage == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Imagem não encontrada!");
                }

                await _produtosRepository.DeleteImageAsync(imageId);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK,
                    "Imagem removida com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    $"Erro ao remover imagem: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetSubcategories()
        {
            var subcategories = await _subcategoriaRepository.GetAllAsync();

            if (subcategories.Count == 0)
            {
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT,
                    "Não há subcategorias disponíveis.");
            }

            var subcategoriesResponse = subcategories.Select(subcategoria => subcategoria.ToResponse());

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { subcategories = subcategoriesResponse }));
        }

        public async Task<IActionResult> CreateSubcategory(ProdutoSubcategoria subcategoria)
        {
            await _subcategoriaRepository.CreateAsync(subcategoria);

            return ResponseHelper.CreateSuccessResponse(
                ResponseSuccess.CREATED.WithData(new { subcategoria = subcategoria.ToResponse() }));
        }

        public async Task BatchUpdateProductInfo(RequestBatchUpdateProduto request)
        {
            var product = await _produtosRepository.GetProductByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException("Produto não encontrado!");
            }

            var transaction = _repositoryDapper.BeginTransaction();
            try
            {
                await _produtosRepository.BatchUpdateProductAsync(request, transaction);

                var variations = request.Variations;
                if (variations != null)
                {
                    await _produtosRepository.BatchUpdateVariationsAsync(product.Id, variations, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        // Métodos Auxiliares para Resolução de ID vs Nome
        
        private async Task<Guid> ResolveCategoryIdAsync(string input)
        {
            if (Guid.TryParse(input, out var guid))
            {
                // TODO: Validar se categoria existe (opcional, pode deixar o FK do banco pegar)
                return guid;
            }

            // Tenta buscar por nome
            var categoryId = await _produtosRepository.GetCategoryIdByNameAsync(input);
            if (categoryId == null)
            {
                throw new ArgumentException($"Categoria '{input}' não encontrada.");
            }

            return categoryId.Value;
        }

        private async Task<Guid?> ResolveSubcategoryIdAsync(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            if (Guid.TryParse(input, out var guid))
            {
                return guid;
            }

            // Tenta buscar por nome
            var subcategoryId = await _produtosRepository.GetSubcategoryIdByNameAsync(input);
            if (subcategoryId == null)
            {
                throw new ArgumentException($"Subcategoria '{input}' não encontrada.");
            }

            return subcategoryId;
        }
    }
}
