using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface ITipoProgressoRepository
    {
        Task<List<TipoProgresso>> GetAllAsync();
        Task<TipoProgresso?> GetByIdAsync(Guid id);
        Task<TipoProgresso?> GetByNomeAsync(string nome);
        Task<bool> CreateAsync(TipoProgresso entity);
        Task<bool> UpdateAsync(Guid id, TipoProgresso entity);
        Task<bool> DeleteAsync(Guid id);
    }

    public class TipoProgressoRepository : BaseRepository<TipoProgresso>, ITipoProgressoRepository
    {
        public TipoProgressoRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<TipoProgresso?> GetByNomeAsync(string nome)
        {
            var sql = "SELECT id AS Id, nome AS Nome FROM tipos_progresso WHERE LOWER(nome) = LOWER(@nome)";
            var result = await _dapper.QueryAsync<TipoProgresso>(sql, new { nome });
            return result.FirstOrDefault();
        }
    }
}
