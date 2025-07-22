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
                throw new Exception("Erro ao obter todos os diretores");
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
                    Usuario_id = diretor.Usuario_id,
                    Diretoria_id = diretor.Diretoria_id,
                    Email =  diretor.Email,
                    Github_link = diretor.Github_link,
                    Linkedin_link = diretor.Linkedin_link
                    
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar diretor.");
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
                throw new Exception("Erro ao deletar diretor.");
            }
        }

        public async Task UpdateDiretor(int id, RequestDiretor diretor)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateDiretor");
                var param = new
                {
                    Nome = diretor.Nome,
                    Descricao = diretor.Descricao,
                    Imagem_url = diretor.Imagem_url,
                    Diretoria_id = diretor.Diretoria_id,
                    Email = diretor.Email,
                    Github_link = diretor.Github_link,
                    Linkedin_link = diretor.Linkedin_link
                    
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar diretor.");
            };
        }

        public async Task<Diretor?> GetDiretorById(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetDiretorById");

                var queryResult = await _repositoryDapper.QueryAsync<Diretor>(sql);

                var diretor = queryResult.FirstOrDefault();
                return diretor;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter diretor.");
            }
        }
    }
}
