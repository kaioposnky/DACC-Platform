using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public interface IEventosRepository
    {
        Task<List<Evento>> GetAllAsync();
        Task<Evento?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Evento entity);
        Task<bool> UpdateAsync(Guid id, Evento entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
