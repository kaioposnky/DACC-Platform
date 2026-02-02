using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class ProjetosRepository : BaseRepository<Projeto>, IProjetosRepository
    {
        public ProjetosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }
        public async Task<(List<Projeto> Projetos, int TotalCount)> SearchProjetos(DaccApi.Model.Requests.RequestQueryProjeto query)
        {
            var sql = _dapper.GetQueryNamed("SearchProjetos");
            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page,
                Limit = query.Limit,
                Status = query.Status,
                Directorate = query.Directorate,
                MinProgress = query.MinProgress,
                MaxProgress = query.MaxProgress
            };

            var result = (await _dapper.QueryAsync<Projeto>(sql, queryParams)).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }
    }
}
