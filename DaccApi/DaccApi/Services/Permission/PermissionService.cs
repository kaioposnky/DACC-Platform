using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Repositories.Permission;
using Microsoft.Extensions.Caching.Memory;

namespace DaccApi.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMemoryCache _memoryCache;

        public PermissionService(IPermissionRepository permissionRepository, IMemoryCache memoryCache)
        {
            _permissionRepository = permissionRepository;
            _memoryCache = memoryCache;
        }
        
        public async Task<HashSet<string>> GetPermissionsForRoleAsync(string role)
        {
            var cacheKey = "permissions" + role;

            return (await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                
                return await _permissionRepository.GetPermissionsForRoleAsync(role);
            }))!;
        }
    }
}