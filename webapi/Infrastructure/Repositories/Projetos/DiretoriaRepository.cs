using DaccApi.Infrastructure.Dapper;
using DaccApi.Data.Orm;
using DaccApi.Model.Objects;
using DaccApi.Model.Requests.Projetos;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class DiretoriaRepository : BaseRepository<Diretoria>, IDiretoriaRepository
    {
        public DiretoriaRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<(List<Diretoria> Directorates, int TotalCount)> SearchAsync(RequestQueryDiretoria query)
        {
            var sql = @"
                SELECT 
                    id as Id,
                    nome as Nome,
                    descricao as Descricao,
                    COUNT(*) OVER() as TotalCount
                FROM diretoria
                WHERE 1=1
                AND (@SearchPattern IS NULL OR nome ILIKE @SearchPattern OR descricao ILIKE @SearchPattern)
                ORDER BY nome ASC
                OFFSET (@Page - 1) * @Limit
                LIMIT @Limit";

            var queryParams = new
            {
                SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Page = query.Page ?? 1,
                Limit = query.Limit ?? 16
            };

            var result = await _dapper.QueryAsync<Diretoria>(sql, queryParams);
            var list = result.ToList();
            
            var totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
            
            return (list, totalCount);
        }
    }
}
