using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Permission
{
    /// <summary>
    /// Implementação do repositório de permissões.
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissionRepository"/>.
        /// </summary>
        public PermissionRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }
        
        /// <summary>
        /// Obtém um conjunto de permissões para um determinado perfil (role).
        /// </summary>
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