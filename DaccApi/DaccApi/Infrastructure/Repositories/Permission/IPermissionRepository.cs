using DaccApi.Enum.UserEnum;

namespace DaccApi.Infrastructure.Repositories.Permission
{
    public interface IPermissionRepository
    {
        public Task<HashSet<string>> GetPermissionsForRoleAsync(TipoUsuarioEnum role);
    }
}