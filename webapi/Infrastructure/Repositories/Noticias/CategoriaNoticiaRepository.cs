using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    public interface ICategoriaNoticiaRepository
    {
        Task<List<CategoriaNoticia>> GetAllAsync();
        Task<CategoriaNoticia?> GetByIdAsync(Guid id);
        Task<CategoriaNoticia?> GetByNomeAsync(string nome);
        Task<bool> CreateAsync(CategoriaNoticia entity);
        Task<bool> UpdateAsync(Guid id, CategoriaNoticia entity);
        Task<bool> DeleteAsync(Guid id);
    }

    public class CategoriaNoticiaRepository : BaseRepository<CategoriaNoticia>, ICategoriaNoticiaRepository
    {
        public CategoriaNoticiaRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<CategoriaNoticia?> GetByNomeAsync(string nome)
        {
            var sql = "SELECT id AS Id, nome AS Nome FROM categorias_noticia WHERE LOWER(nome) = LOWER(@nome)";
            var result = await _dapper.QueryAsync<CategoriaNoticia>(sql, new { nome });
            return result.FirstOrDefault();
        }
    }
}
