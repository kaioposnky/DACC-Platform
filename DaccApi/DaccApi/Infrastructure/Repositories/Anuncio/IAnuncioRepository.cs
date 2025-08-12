
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    public interface IAnuncioRepository
    {
    
        public Task<List<Model.Anuncio>> GetAllAnuncio();

        public Task<Model.Anuncio> GetAnuncioById(Guid id);
        public Task CreateAnuncio(RequestAnuncio anuncio);

        public Task DeleteAnuncio(Guid id);
        public Task UpdateAnuncio(Guid id, Model.Anuncio anuncio);
        
        
    }
}

