using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public interface IEventosRepository
    {
        public Task<List<Evento>> GetAllEventos();
        public Task<Evento?> GetEventoById(int id);
        public Task CreateEvento(RequestEvento evento);
        public Task DeleteEvento(int id);
    
        public Task UpdateEvento(int id, RequestEvento evento);
    }
    
}
