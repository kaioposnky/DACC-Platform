using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public interface INoticiasRepository
{
    public Task<List<Noticia>> GetAllNoticias();
    public Task CreateNoticia(RequestNoticia noticia);
    public Task DeleteNoticia(Guid id);
    public Task<Noticia?> GetNoticiaById(Guid id);
    public Task UpdateNoticia(Guid id, RequestNoticia noticia);
}