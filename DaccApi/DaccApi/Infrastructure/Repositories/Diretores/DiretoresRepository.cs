using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    public class DiretoresRepository : IDiretoresRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public DiretoresRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<List<Diretor>> GetAllDiretores()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllDiretores");

                var queryResult = await _repositoryDapper.QueryAsync<Diretor>(sql);

                var diretores = queryResult.ToList();
                return diretores;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os diretores" + ex.Message);
            }
        }

        public async Task CreateDiretor(RequestDiretor diretor)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateDiretor");
            
                var param = new
                {
                    Nome = diretor.Nome,
                    Descricao = diretor.Descricao,
                    UsuarioId = diretor.UsuarioId,
                    DiretoriaId = diretor.DiretoriaId,
                    Email =  diretor.Email,
                    GithubLink = diretor.GithubLink,
                    LinkedinLink = diretor.LinkedinLink
                    
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar diretor." + ex.Message);
            }
        }


        public async Task DeleteDiretor(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteDiretor");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar diretor." + ex.Message);
            }
        }

        public async Task UpdateDiretor(int id, RequestDiretor diretor)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateDiretor");
                var param = new
                {
                    id = id,
                    Nome = diretor.Nome,
                    Descricao = diretor.Descricao,
                    ImagemUrl = diretor.ImagemUrl,
                    DiretoriaId = diretor.DiretoriaId,
                    Email = diretor.Email,
                    GithubLink = diretor.GithubLink,
                    LinkedinLink = diretor.LinkedinLink
                    
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar diretor." + ex.Message);
            };
        }

        public async Task<Diretor?> GetDiretorById(int id)
        {
            try
            {
                var sql =  _repositoryDapper.GetQueryNamed("GetDiretorById");
                
                var param = new { id = id };

                var queryResult = await _repositoryDapper.QueryAsync<Diretor>(sql,param);

                var diretor = queryResult.FirstOrDefault();

                return diretor;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter diretor." + ex.Message);
            }
        }
    }
}
