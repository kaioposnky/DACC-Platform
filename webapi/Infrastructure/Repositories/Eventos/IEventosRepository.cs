using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    /// <summary>
    /// Define a interface para o repositório de eventos.
    /// </summary>
    public interface IEventosRepository
    {
        /// <summary>
        /// Obtém todos os eventos.
        /// </summary>
        public Task<List<Evento>> GetAllEventos();
        /// <summary>
        /// Obtém um evento específico pelo seu ID.
        /// </summary>
        public Task<Evento?> GetEventoById(Guid id);
        /// <summary>
        /// Cria um novo evento.
        /// </summary>
        public Task<Guid> CreateEvento(Evento evento);
        /// <summary>
        /// Deleta um evento existente.
        /// </summary>
        public Task DeleteEvento(Guid id);
        /// <summary>
        /// Atualiza um evento existente.
        /// </summary>
        public Task UpdateEvento(Guid id, Evento evento);
    }
    
}
