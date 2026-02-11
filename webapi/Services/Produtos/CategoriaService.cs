using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Produtos;
using DaccApi.Model.Responses.Produto;
using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Produtos
{
    /// <summary>
    /// Implementação do serviço de categorias de produtos.
    /// </summary>
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IActionResult> GetAllCategoriasAsync()
        {
            try
            {
                var categorias = await _categoriaRepository.GetAllCategoriasAsync();
                var response = categorias.Select(c => new ResponseCategoria(c)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { categories = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetAllSubcategoriasAsync()
        {
            try
            {
                var subcategorias = await _categoriaRepository.GetAllSubcategoriasAsync();
                var response = subcategorias.Select(s => new ResponseSubcategoria(s)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { subcategories = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
