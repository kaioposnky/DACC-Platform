using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Objects;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.User
{
    public class CursoRepository : ICursoRepository
    {
        private readonly IRepositoryDapper _dapper;

        public CursoRepository(IRepositoryDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<Curso>> GetAllAsync()
        {
            var sql = _dapper.GetQueryNamed("GetAllCursos");
            var result = await _dapper.QueryAsync<Curso>(sql);
            return result.ToList();
        }

        public async Task<Curso?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetCursoById");
            var result = await _dapper.QueryAsync<Curso>(sql, new { id });
            return result.FirstOrDefault();
        }

        public async Task<Curso?> GetByNomeAsync(string nome)
        {
            var sql = _dapper.GetQueryNamed("GetCursoByNome");
            var result = await _dapper.QueryAsync<Curso>(sql, new { nome });
            return result.FirstOrDefault();
        }

        public async Task<Guid> CreateAsync(Curso curso)
        {
            var sql = "INSERT INTO curso (nome) VALUES (@Nome) RETURNING id";
            return await _dapper.QueryFirstAsync<Guid>(sql, new { curso.Nome });
        }

        public async Task UpdateAsync(Guid id, Curso curso)
        {
            var sql = "UPDATE curso SET nome = @Nome WHERE id = @id";
            await _dapper.ExecuteAsync(sql, new { id, curso.Nome });
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM curso WHERE id = @id";
            await _dapper.ExecuteAsync(sql, new { id });
        }
    }
}
