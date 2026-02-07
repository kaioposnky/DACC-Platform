using DaccApi.Infrastructure.Dapper;
using DaccApi.Data.Orm;
using DaccApi.Model;
using DaccApi.Model.Objects;
using DaccApi.Model.Requests;

namespace DaccApi.Infrastructure.Repositories.Professores
{
    /// <summary>
    /// Implementação do repositório de professores (faculty).
    /// </summary>
    public class ProfessoresRepository : BaseRepository<Professor>, IProfessoresRepository
    {
        public ProfessoresRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<(List<Professor> Professores, int TotalCount)> SearchProfessores(RequestQueryProfessor query)
        {
            var sql = _dapper.GetQueryNamed("SearchProfessores");
            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page,
                Limit = query.Limit
            };

            var result = (await _dapper.QueryAsync<Professor>(sql, queryParams)).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }
    }
}
