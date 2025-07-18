using DaccApi.Enum.UserEnum;

namespace DaccApi.Services.Permission
{
    public interface IPermissionService
    {
        public Task<HashSet<string>> GetPermissionsForRoleAsync(TipoUsuarioEnum role);
    }
}