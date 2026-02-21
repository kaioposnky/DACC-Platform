using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Produtos
{
    /// <summary>
    /// Interface para serviço de categorias de produtos.
    /// </summary>
    public interface ICategoriaService
    {
        Task<IActionResult> GetAllCategoriasAsync();
        Task<IActionResult> GetAllSubcategoriasAsync();
    }
}
