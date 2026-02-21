using DaccApi.Model;

namespace DaccApi.Data.Orm.Subcategoria;

public interface ISubcategoriaRepository
{
    Task<List<ProdutoSubcategoria>> GetAllAsync();
    Task<ProdutoSubcategoria> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, ProdutoSubcategoria entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> CreateAsync(ProdutoSubcategoria subcategoria);
}