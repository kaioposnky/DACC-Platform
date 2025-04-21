using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretorias
{
    public class DiretoriasRepository : IDiretoriasRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public async Task<List<Diretoria>> GetAllDiretoriasAsync()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllDiretorias");

                var queryResult = await _repositoryDapper.QueryProcedureAsync<Diretoria>(sql);

                var diretorias = queryResult.ToList();
                return diretorias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter diretorias no banco de dados!");
            }
        }

    }
}
