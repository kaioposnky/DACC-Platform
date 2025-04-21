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
        public async Task<List<Projeto>> GetAllProjetosAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProjetos");

            var queryResult = await _repositoryDapper.QueryProcedureAsync<Projeto>(sql);

            var projetos = queryResult.ToList();

            return projetos;
        }

    }
}
