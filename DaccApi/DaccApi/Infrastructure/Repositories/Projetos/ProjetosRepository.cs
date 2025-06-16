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
        public List<Projeto> GetAllProjetos()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProjetos");

            var queryResult = _repositoryDapper.Query<Projeto>(sql);

            var projetos = queryResult.ToList();

            return projetos;
        }

    }
}
