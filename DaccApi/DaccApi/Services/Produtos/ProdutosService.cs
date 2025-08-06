using DaccApi.Model;
using DaccApi.Model.Responses;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Helpers;
using Helpers.Response;

namespace DaccApi.Services.Products
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProdutosService(IProdutosRepository produtosRepository, IFileStorageService fileStorageService)
        {
            _produtosRepository = produtosRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _produtosRepository.GetAllProductsAsync();

                if (products.Count == 0)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, 
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
                var product = await CreateProductEntityAsync(requestCreateProduto, productId);

                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(new { product }), 
                    "Produto criado com sucesso! Use o endpoint de variações para adicionar opções de compra.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    $"Erro ao criar produto: {ex.Message}");
            }
        }

        private async Task<Produto> CreateProductEntityAsync(RequestCreateProduto request, Guid productId)
        {
            var product = Produto.FromRequest(request, productId);
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
                var products = await _produtosRepository.SearchProductsAsync(requestQueryProdutos);

                if (products.Count == 0)
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Nenhum produto encontrado com os critérios de busca!");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { products }), 
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

                var variationExists = await _produtosRepository.VariationExistsAsync(productId, request.Cor.Trim(), request.Tamanho);
                if (variationExists)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_ALREADY_EXISTS,
                        $"Já existe uma variação com cor '{request.Cor}' e tamanho '{request.Tamanho}' para este produto!");
                }

                var variationId = Guid.NewGuid();
                var sku = ProdutoVariacao.GenerateSku(productId, request.Cor.Trim(), request.Tamanho);
                var variation = ProdutoVariacao.FromRequest(request, productId, variationId, sku);

                await _produtosRepository.CreateProductVariationAsync(variation);

                var createdVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                var response = Produto.MapToResponseVariacao(createdVariation ?? variation);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(new { variation = response }),
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

                if ((request.Cor != null && request.Cor.Trim() != existingVariation.Cor) || 
                    (request.Tamanho != null && request.Tamanho != existingVariation.Tamanho))
                {
                    var newCor = request.Cor?.Trim() ?? existingVariation.Cor;
                    var newTamanho = request.Tamanho ?? existingVariation.Tamanho;
                    
                    var variationExists = await _produtosRepository.VariationExistsAsync(productId, newCor.Trim(), newTamanho);
                    if (variationExists)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_ALREADY_EXISTS,
                            $"Já existe uma variação com cor '{newCor}' e tamanho '{newTamanho}' para este produto!");
                    }
                }

                existingVariation.UpdateFromRequest(request);
                
                if (request.Cor != null || request.Tamanho != null)
                {
                    existingVariation.Sku = ProdutoVariacao.GenerateSku(productId, existingVariation.Cor, existingVariation.Tamanho);
                }

                existingVariation.DataAtualizacao = DateTime.UtcNow;

                await _produtosRepository.UpdateVariationAsync(existingVariation);

                var updatedVariation = await _produtosRepository.GetVariationByIdAsync(variationId);
                var response = Produto.MapToResponseVariacao(updatedVariation ?? existingVariation);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.OK.WithData(new { variation = response }),
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
                    Console.WriteLine($"Warning: Failed to delete variation images: {ex.Message}");
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

                var hasChanges = false;
                if (requestUpdateProduto.Nome != null)
                {
                    product.Nome = requestUpdateProduto.Nome;
                    hasChanges = true;
                }

                if (requestUpdateProduto.Descricao != null)
                {
                    product.Descricao = requestUpdateProduto.Descricao;
                    hasChanges = true;
                }

                if (requestUpdateProduto.Categoria != null)
                {
                    product.Categoria = requestUpdateProduto.Categoria;
                    hasChanges = true;
                }

                if (requestUpdateProduto.Subcategoria != null)
                {
                    product.Subcategoria = requestUpdateProduto.Subcategoria;
                    hasChanges = true;
                }

                if (requestUpdateProduto.Preco.HasValue)
                {
                    product.Preco = requestUpdateProduto.Preco;
                    hasChanges = true;
                }

                if (requestUpdateProduto.PrecoOriginal.HasValue)
                {
                    product.PrecoOriginal = requestUpdateProduto.PrecoOriginal;
                    hasChanges = true;
                }

                if (!hasChanges)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Nenhum campo para atualização foi fornecido.");
                }

                product.DataAtualizacao = DateTime.UtcNow;

                await _produtosRepository.UpdateProductAsync(product);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Produto atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, $"Erro ao atualizar produto: {ex.Message}");
            }
        }
        
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

                var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);
                var produtoImagem = new ProdutoImagem
                {
                    Id = Guid.NewGuid(),
                    ProdutoVariacaoId = variationId,
                    ImagemUrl = imageUrl,
                    ImagemAlt = request.ImagemAlt?.Trim(),
                    Ordem = request.Ordem,
                };

                await _produtosRepository.AddProductImagesAsync(produtoImagem);
                
                return ResponseHelper.CreateSuccessResponse(
                    ResponseSuccess.CREATED.WithData(new { image = produtoImagem }), 
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

                if (request.Imagem != null)
                {
                    existingImage.ImagemUrl = await _fileStorageService.SaveImageFileAsync(request.Imagem);
                }

                if (request.Ordem.HasValue)
                {
                    existingImage.Ordem = request.Ordem.Value;
                }

                if (request.ImagemAlt != null)
                {
                    existingImage.ImagemAlt = request.ImagemAlt.Trim();
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
    }
}
