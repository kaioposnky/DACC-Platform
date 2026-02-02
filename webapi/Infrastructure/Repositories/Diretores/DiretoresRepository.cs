using DaccApi.Infrastructure.Dapper;
using DaccApi.Data.Orm;
using DaccApi.Model;
using DaccApi.Model.Objects;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    /// <summary>
    /// Implementação do repositório de diretores.
    /// </summary>
    public class DiretoresRepository : BaseRepository<Diretor>, IDiretoresRepository
    {
        public DiretoresRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<(List<Diretor> Diretores, int TotalCount)> SearchDiretores(Model.Requests.RequestQueryDiretor query)
        {
            var sql = _dapper.GetQueryNamed("SearchDiretores");
            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page,
                Limit = query.Limit
            };

            var result = (await _dapper.QueryAsync<Diretor>(sql, queryParams)).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }
    }
}
