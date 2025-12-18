using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public interface INoticiasRepository
{
    Task<List<Noticia>> GetAllAsync();
    Task<Noticia?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(Noticia entity);
    Task<bool> UpdateAsync(Guid id, Noticia entity);
    Task<bool> DeleteAsync(Guid id);
}