using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public interface INoticiasRepository
{
    public Task<List<Noticia>> GetAllNoticias();
    public Task CreateNoticia(RequestNoticia noticia);
    public Task DeleteNoticia(int id);
    public Task<Noticia?> GetNoticiaById(int id);
    public Task UpdateNoticia(int id, RequestNoticia noticia);
}