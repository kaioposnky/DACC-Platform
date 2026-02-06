using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Eventos
{
    public interface ITipoEventoRepository
    {
        Task<List<TipoEvento>> GetAllAsync();
        Task<TipoEvento?> GetByIdAsync(Guid id);
        Task<TipoEvento?> GetByNomeAsync(string nome);
        Task<bool> CreateAsync(TipoEvento entity);
        Task<bool> UpdateAsync(Guid id, TipoEvento entity);
        Task<bool> DeleteAsync(Guid id);
    }

    public class TipoEventoRepository : BaseRepository<TipoEvento>, ITipoEventoRepository
    {
        public TipoEventoRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<TipoEvento?> GetByNomeAsync(string nome)
        {
            var sql = "SELECT id AS Id, nome AS Nome FROM tipos_evento WHERE LOWER(nome) = LOWER(@nome)";
            var result = await _dapper.QueryAsync<TipoEvento>(sql, new { nome });
            return result.FirstOrDefault();
        }
    }
}
