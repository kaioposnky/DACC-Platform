using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public interface INoticiasRepository
{
    public Task<List<Noticia>> GetAllNoticias();
    public Task CreateNoticia(RequestNoticia noticia);
}