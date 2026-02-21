using DaccApi.Model;
using DaccApi.Model.Objects.Noticia;
using DaccApi.Model.Responses;

namespace DaccApi.Services.Home
{
    public interface IHomeService
    {
        Task<object> GetUnifiedFeed(int limit);
    }
}
