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
            
            var result = await _dapper.QueryAsync<dynamic>(sql);
            var projetos = result.Select(row => new Projeto
            {
                Id = row.id,
                Titulo = row.titulo,
                Descricao = row.descricao,
                ImagemUrl = row.imagemurl,
                Status = row.status,
                Tags = row.tags,
                Progresso = row.progresso,
                TextoConclusao = row.textoconclusao,
                Departamento = ((object)row.dept_id) != null ? new Diretoria
                {
                    Id = row.dept_id,
                    Nome = row.dept_nome,
                    Descricao = row.dept_descricao
                } : null
            }).ToList();
            
            return projetos;
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


            var result = await _dapper.QueryAsync<dynamic>(sql, queryParams);
            var projetos = result.Select(row => new Projeto
            {
                Id = row.id,
                Titulo = row.titulo,
                Descricao = row.descricao,
                ImagemUrl = row.imagemurl,
                Status = row.status,
                Tags = row.tags,
                Progresso = row.progresso,
                TextoConclusao = row.textoconclusao,
                DataCriacao = row.datacriacao,
                DataAtualizacao = row.dataatualizacao,
                TotalCount = (int)(row.totalcount ?? 0),
                Departamento = ((object)row.dept_id) != null ? new Diretoria
                {
                    Id = row.dept_id,
                    Nome = row.dept_nome,
                    Descricao = row.dept_descricao
                } : null
            }).ToList();

            var totalCount = 0;
            if (projetos.Any())
            {
                totalCount = projetos.First().TotalCount;
            }

            return (projetos, totalCount);
        }

        public new async Task<Projeto?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetProjetoById");
            var param = new { id = id };

            var result = await _dapper.QueryAsync<dynamic>(sql, param);
            var projeto = result.Select(row => new Projeto
            {
                Id = row.id,
                Titulo = row.titulo,
                Descricao = row.descricao,
                ImagemUrl = row.imagemurl,
                Status = row.status,
                Tags = row.tags,
                Progresso = row.progresso,
                TextoConclusao = row.textoconclusao,
                Departamento = ((object)row.dept_id) != null ? new Diretoria
                {
                    Id = row.dept_id,
                    Nome = row.dept_nome,
                    Descricao = row.dept_descricao
                } : null
            }).FirstOrDefault();

            return projeto;
        }
    }
}
