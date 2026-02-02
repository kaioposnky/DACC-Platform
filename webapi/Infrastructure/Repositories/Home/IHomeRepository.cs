using DaccApi.Model;
using DaccApi.Model.Objects.Noticia;

namespace DaccApi.Infrastructure.Repositories.Home
{
    public interface IHomeRepository
    {
        Task<(List<Noticia> Noticias, List<Evento> Eventos, List<Projeto> Projetos)> GetUnifiedFeedAsync(int limit);
    }
}
