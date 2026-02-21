using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public class EventosRepository : BaseRepository<Evento>, IEventosRepository
    {
        public EventosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }
        public async Task<(List<Evento> Eventos, int TotalCount)> SearchEventos(DaccApi.Model.Requests.RequestQueryEvento query)
        {
            var sql = _dapper.GetQueryNamed("SearchEventos");
            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page,
                Limit = query.Limit,
                Type = query.Type,
                DateFrom = query.DateFrom.HasValue ? DateTime.SpecifyKind(query.DateFrom.Value, DateTimeKind.Utc) : (DateTime?)null,
                DateTo = query.DateTo.HasValue ? DateTime.SpecifyKind(query.DateTo.Value, DateTimeKind.Utc) : (DateTime?)null
            };

            var result = (await _dapper.QueryAsync<Evento, Usuario, Evento>(
                sql,
                (evento, usuario) =>
                {
                    evento.Autor = usuario;
                    return evento;
                },
                queryParams,
                splitOn: "Usuario_Id"
            )).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }

        public new async Task<Evento?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetEventoById");
            var param = new { Id = id };

            var result = await _dapper.QueryAsync<Evento, Usuario, Evento>(
                sql,
                (evento, usuario) =>
                {
                    evento.Autor = usuario;
                    return evento;
                },
                param,
                splitOn: "Usuario_Id"
            );

            return result.FirstOrDefault();
        }
    }
}