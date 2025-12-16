
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    /// <summary>
    /// Define a interface para o repositório de anúncios.
    /// </summary>
    public interface IAnuncioRepository
    {
    
        /// <summary>
        /// Obtém todos os anúncios.
        /// </summary>
        public Task<List<Model.Anuncio>> GetAllAnuncio();

        /// <summary>
        /// Obtém um anúncio específico pelo seu ID.
        /// </summary>
        public Task<Model.Anuncio> GetAnuncioById(Guid id);

        /// <summary>
        /// Cria um novo anúncio.
        /// </summary>
        public Task CreateAnuncio(RequestAnuncio anuncio);

        /// <summary>
        /// Deleta um anúncio existente.
        /// </summary>
        public Task DeleteAnuncio(Guid id);

        /// <summary>
        /// Atualiza um anúncio existente.
        /// </summary>
        public Task UpdateAnuncio(Guid id, Model.Anuncio anuncio);
        
        
    }
}
