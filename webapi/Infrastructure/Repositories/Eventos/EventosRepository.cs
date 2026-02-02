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
        public async Task<(List<Evento> Eventos, int TotalCount)> SearchEventos(DaccApi.Model.Requests.RequestQueryEvento query)
        {
            var sql = _dapper.GetQueryNamed("SearchEventos");
            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page,
                Limit = query.Limit,
                Type = query.Type,
                DateFrom = query.DateFrom,
                DateTo = query.DateTo
            };

            var result = (await _dapper.QueryAsync<Evento>(sql, queryParams)).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }
    }
}