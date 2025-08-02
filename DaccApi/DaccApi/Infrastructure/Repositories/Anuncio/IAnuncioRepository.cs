
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    public interface IAnuncioRepository
    {
    
        public Task<List<Anunci>> GetAllAnuncio();

        public Task<Anunci?> GetAnuncioById(Guid id);
        public Task CreateAnuncio(RequestAnuncio anuncio);

        public Task DeleteAnuncio(Guid id);
        public Task UpdateAnuncio(Guid id, RequestAnuncio anuncio);
        
        
    }
}

