using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    public class TipoAnuncioRepository : BaseRepository<TipoAnuncio>, ITipoAnuncioRepository
    {
        public TipoAnuncioRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<TipoAnuncio?> GetByNomeAsync(string nome)
        {
            var sql = "SELECT id AS Id, nome AS Nome FROM tipos_anuncio WHERE LOWER(nome) = LOWER(@nome)";
            var result = await _dapper.QueryAsync<TipoAnuncio>(sql, new { nome });
            return result.FirstOrDefault();
        }
    }
}
