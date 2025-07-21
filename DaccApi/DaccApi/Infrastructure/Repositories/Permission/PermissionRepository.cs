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
        
        public async Task<HashSet<string>> GetPermissionsForRoleAsync(string role)
        {
            var sql = _repositoryDapper.GetQueryNamed("GetPermissionsForRole");

            var param = new
            {
                RoleName = role
            };
            
            var permissions = await _repositoryDapper.QueryAsync<string>(sql, param);
            return new HashSet<string>(permissions);
        }
    }
}