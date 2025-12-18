using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public class EventosRepository : BaseRepository<Evento>, IEventosRepository
    {
        public EventosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }
    }
}