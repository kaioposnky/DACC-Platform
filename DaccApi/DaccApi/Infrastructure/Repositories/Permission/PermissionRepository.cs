using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Permission
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        public PermissionRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }
        
        public async Task<HashSet<string>> GetPermissionsForRoleAsync(TipoUsuarioEnum role)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetPermissionsForRole");
            
            var roleName = TiposUsuario.FromEnum(role);
            
            var param = new
            {
                RoleName = roleName
            };
            
            var permissions = await _repositoryDapper.QueryAsync<string>(sql, param);
            return new HashSet<string>(permissions);
        }
    }
}