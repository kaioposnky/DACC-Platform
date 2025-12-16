using System.Threading.Tasks;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    /// <summary>
    /// Implementação do repositório de projetos.
    /// </summary>
    public class ProjetosRepository : IProjetosRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProjetosRepository"/>.
        /// </summary>
        public ProjetosRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }
        /// <summary>
        /// Obtém todos os projetos.
        /// </summary>
        public async Task<List<Projeto>> GetAllProjetos()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllProjetos");

            var queryResult = await _repositoryDapper.QueryAsync<Projeto>(sql);

            var projetos = queryResult.ToList();

            return projetos;
        }

        /// <summary>
        /// Obtém um projeto específico pelo seu ID.
        /// </summary>
        public async Task<Projeto?> GetProjetoById(Guid id)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProjetoById");
            
            var queryResult = await _repositoryDapper.QueryAsync<Projeto>(sql, new { id = id });
            
            var projeto = queryResult.FirstOrDefault();
            return projeto;
        }

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        public async Task CreateProjeto(Projeto projeto)
        {
            try
            {
                
                var sql = _repositoryDapper.GetQueryNamed("CreateProjeto");
            
                var param = new
                {
                    Titulo = projeto.Titulo,
                    Descricao = projeto.Descricao,
                    Status = projeto.Status,
                    Diretoria = projeto.Diretoria,
                    Tags = projeto.Tags


                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar projeto." + ex.Message);
            }
        }

        /// <summary>
        /// Deleta um projeto existente.
        /// </summary>
        public async Task DeleteProjeto(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteProjeto");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar projeto." + ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um projeto existente.
        /// </summary>
        public async Task UpdateProjeto(Guid id, Projeto projeto)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateProjeto");
                var param = new
                {
                    id = id,
                    Titulo = projeto.Titulo,
                    Descricao = projeto.Descricao,
                    Status = projeto.Status,
                    Diretoria = projeto.Diretoria,
                    Tags = projeto.Tags
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
                
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar projeto." + ex.Message);
            };
        }
        
    }
}
