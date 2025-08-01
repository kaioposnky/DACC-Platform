using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public interface IEventosRepository
    {
        public Task<List<Evento>> GetAllEventos();
        public Task<Evento?> GetEventoById(Guid id);
        public Task CreateEvento(RequestEvento evento);
        public Task DeleteEvento(Guid id);
    
        public Task UpdateEvento(Guid id, RequestEvento evento);
    }
    
}
