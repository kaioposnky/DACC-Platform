using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    public interface ITipoAnuncioRepository
    {
        Task<List<TipoAnuncio>> GetAllAsync();
        Task<TipoAnuncio?> GetByIdAsync(Guid id);
        Task<TipoAnuncio?> GetByNomeAsync(string nome);
        Task<bool> CreateAsync(TipoAnuncio tipoAnuncio);
        Task<bool> UpdateAsync(Guid id, TipoAnuncio tipoAnuncio);
        Task<bool> DeleteAsync(Guid id);
    }
}
