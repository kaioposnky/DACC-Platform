using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Eventos
{

    public class EventosRepository : IEventosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public EventosRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<List<Evento>> GetAllEventos()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllEventos");

                var queryResult = await _repositoryDapper.QueryAsync<Evento>(sql);

                var eventos = queryResult.ToList();
                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os eventos" + ex.Message);
            }
        }

        public async Task<Evento?> GetEventoById(Guid id)
        {
            try
            {
                var sql =  _repositoryDapper.GetQueryNamed("GetEventoById");
                
                var param = new { id = id };

                var queryResult = await _repositoryDapper.QueryAsync<Evento>(sql,param);

                var evento = queryResult.FirstOrDefault();

                return evento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter evento." + ex.Message);
            }
        }

        public async Task CreateEvento(Evento evento)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateEvento");
            
                var param = new
                {
                    Titulo = evento.Titulo,
                    Descricao = evento.Descricao,
                    TipoEvento = evento.TipoEvento,
                    AutorId = evento.AutorId,
                    TextoAcao =  evento.TextoAcao,
                    LinkAcao = evento.LinkAcao,
                    Data = evento.Data ?? DateTime.UtcNow
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar evento." + ex.Message);
            }
        }

        public async Task DeleteEvento(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteEvento");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar evento." + ex.Message);
            }
        }

        public async Task UpdateEvento(Guid id, Evento evento)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateEvento");
                var param = new
                {
                    id = id,
                    Titulo = evento.Titulo,
                    Descricao = evento.Descricao,
                    TipoEvento = evento.TipoEvento,
                    AutorId = evento.AutorId,
                    TextoAcao =  evento.TextoAcao,
                    LinkAcao = evento.LinkAcao,
                    Data = evento.Data ?? DateTime.UtcNow
                    
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar evento." + ex.Message);
            }; 
        }

    }
}