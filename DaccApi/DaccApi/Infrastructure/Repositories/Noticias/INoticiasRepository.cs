using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public interface INoticiasRepository
{
    public List<Noticia> GetAllNoticias();
}