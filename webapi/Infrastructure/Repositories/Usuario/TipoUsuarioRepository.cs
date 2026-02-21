using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.User
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly IRepositoryDapper _dapper;

        public TipoUsuarioRepository(IRepositoryDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<TipoUsuario>> GetAllAsync()
        {
            var sql = _dapper.GetQueryNamed("GetAllTiposUsuario");
            var result = await _dapper.QueryAsync<TipoUsuario>(sql);
            return result.ToList();
        }
    }
}
