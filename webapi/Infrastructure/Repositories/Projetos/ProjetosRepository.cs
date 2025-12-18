using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class ProjetosRepository : BaseRepository<Projeto>, IProjetosRepository
    {
        public ProjetosRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }
    }
}
