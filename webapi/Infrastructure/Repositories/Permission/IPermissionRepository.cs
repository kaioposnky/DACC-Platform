using DaccApi.Enum.UserEnum;

namespace DaccApi.Infrastructure.Repositories.Permission
{
    /// <summary>
    /// Define a interface para o repositório de permissões.
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Obtém um conjunto de permissões para um determinado perfil (role).
        /// </summary>
        public Task<HashSet<string>> GetPermissionsForRoleAsync(string role);
    }
}