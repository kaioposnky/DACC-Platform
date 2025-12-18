using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;

namespace DaccApi.Infrastructure.Repositories.Anuncio;

public class AnuncioRepository : BaseRepository<Model.Anuncio>, IAnuncioRepository
{
    public AnuncioRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
    {
    }
}