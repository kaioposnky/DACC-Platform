using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Produtos
{
    /// <summary>
    /// Repositório para categorias e subcategorias de produtos.
    /// </summary>
    public interface ICategoriaRepository
    {
        Task<List<ProdutoCategoria>> GetAllCategoriasAsync();
        Task<List<ProdutoSubcategoria>> GetAllSubcategoriasAsync();
        Task<ProdutoCategoria?> GetCategoriaByIdAsync(Guid id);
        Task<ProdutoSubcategoria?> GetSubcategoriaByIdAsync(Guid id);
    }
}
