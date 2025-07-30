using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProdutosService
    {
        public Task<IActionResult> GetAllProductsAsync();
        public Task<IActionResult> GetProductByIdAsync(Guid produtoId);
        public Task<IActionResult> CreateProductAsync(RequestCreateProduto requestCreateProduto);
        public Task<IActionResult> RemoveProductByIdAsync(Guid produtoId);
        public Task<IActionResult> SearchProductsAsync(RequestQueryProdutos requestQueryProdutos);
        public Task<IActionResult> UpdateProductAsync(Guid productId, RequestUpdateProduto requestUpdateProduto);
        public Task<IActionResult> CreateVariationAsync(Guid productId, RequestProdutoVariacaoCreate request);
        public Task<IActionResult> GetVariationsAsync(Guid productId);
        public Task<IActionResult> UpdateVariationAsync(Guid productId, Guid variationId, RequestUpdateProdutoVariacao request);
        public Task<IActionResult> DeleteVariationAsync(Guid productId, Guid variationId);
    }
}