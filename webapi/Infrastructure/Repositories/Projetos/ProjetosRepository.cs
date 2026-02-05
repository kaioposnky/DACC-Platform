using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using DaccApi.Model.Objects;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class ProjetosRepository : BaseRepository<Projeto>, IProjetosRepository
    {
        public ProjetosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }

        public new async Task<List<Projeto>> GetAllAsync()
        {
            var sql = _dapper.GetQueryNamed("GetAllProjetos");
            
            var result = await _dapper.QueryAsync<Projeto, Diretoria, Projeto>(
                sql,
                (projeto, diretoria) =>
                {
                    projeto.Departamento = diretoria;
                    return projeto;
                },
                splitOn: "Diretoria_Id"
            );
            
            return result.ToList();
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
                DirectorateId = query.DirectorateId,
                MinProgress = query.MinProgress,
                MaxProgress = query.MaxProgress
            };


            var result = (await _dapper.QueryAsync<Projeto, Diretoria, Projeto>(
                sql,
                (projeto, diretoria) =>
                {
                    projeto.Departamento = diretoria;
                    return projeto;
                },
                queryParams,
                splitOn: "Diretoria_Id"
            )).ToList();

            var totalCount = 0;
            if (result.Any())
            {
                totalCount = result.First().TotalCount;
            }

            return (result, totalCount);
        }

        public new async Task<Projeto?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetProjetoById");
            var param = new { id = id };

            var result = await _dapper.QueryAsync<Projeto, Diretoria, Projeto>(
                sql,
                (projeto, diretoria) =>
                {
                    projeto.Departamento = diretoria;
                    return projeto;
                },
                param,
                splitOn: "Diretoria_Id"
            );

            return result.FirstOrDefault();
        }
    }
}
