using DaccApi.Infrastructure.Repositories.Home;
using DaccApi.Model.Responses;
using System.Linq;

namespace DaccApi.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public async Task<object> GetUnifiedFeed(int limit)
        {
            var (noticias, eventos, projetos) = await _homeRepository.GetUnifiedFeedAsync(limit);

            var feed = new
            {
                News = noticias.Select(n => new ResponseNoticia(n)).ToList(),
                Events = eventos.Select(e => new ResponseEvento(e)).ToList(),
                Projects = projetos.Select(p => new ResponseProjeto(p)).ToList()
            };

            return feed;
        }
    }
}
