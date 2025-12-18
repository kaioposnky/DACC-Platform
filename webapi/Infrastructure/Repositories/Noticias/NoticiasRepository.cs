using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    public class NoticiasRepository : BaseRepository<Noticia>, INoticiasRepository
    {
        public NoticiasRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }
    }
}