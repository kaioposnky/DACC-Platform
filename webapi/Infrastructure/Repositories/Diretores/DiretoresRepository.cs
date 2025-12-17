using DaccApi.Infrastructure.Dapper;
using DaccApi.Data.Orm;
using DaccApi.Model;
using DaccApi.Model.Objects;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    /// <summary>
    /// Implementação do repositório de diretores.
    /// </summary>
    public class DiretoresRepository : BaseRepository<Diretor>, IDiretoresRepository
    {
        public DiretoresRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }
    }
}
