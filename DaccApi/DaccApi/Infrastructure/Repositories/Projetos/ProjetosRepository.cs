using System.Threading.Tasks;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class ProjetosRepository : IProjetosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        public ProjetosRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }
        public async Task<List<Projeto>> GetAllProjetos()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProjetos");

            var queryResult = await _repositoryDapper.QueryAsync<Projeto>(sql);

            var projetos = queryResult.ToList();

            return projetos;
        }

        public async Task<Projeto?> GetProjetoById(int id)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProjetoById");
            
            var queryResult = await _repositoryDapper.QueryAsync<Projeto>(sql, new { Id = id });
            
            var projeto = queryResult.FirstOrDefault();
            return projeto;
        }

        public async Task CreateProjeto(RequestProjeto projeto)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateProjeto");
            
                var param = new
                {
                    Titulo = projeto.Titulo,
                    Descricao = projeto.Descricao,
                    ImagemUrl = projeto.ImagemUrl,
                    Status = projeto.Status,
                    Diretoria = projeto.Diretoria,
                    Tags = projeto.Tags


                };
                Console.WriteLine("SQL: " + sql);
                Console.WriteLine("Params: " + param);
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar projeto.",ex);
            }
        }

        public async Task DeleteProjeto(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteProjeto");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar projeto.");
            }
        }

        public async Task UpdateProjeto(int id, RequestProjeto projeto)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateProjeto");
                var param = new
                {
                    id = id,
                    Titulo = projeto.Titulo,
                    Descricao = projeto.Descricao,
                    ImagemUrl = projeto.ImagemUrl,
                    Status = projeto.Status,
                    Diretoria = projeto.Diretoria,
                    Tags = projeto.Tags
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
                
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar projeto.");
            };
        }
        
    }
}
