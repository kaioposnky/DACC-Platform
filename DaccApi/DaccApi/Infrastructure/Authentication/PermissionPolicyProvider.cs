using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DaccApi.Infrastructure.Authentication
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        
        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await _fallbackProvider.GetPolicyAsync(policyName);
            if (policy != null) return await Task.FromResult(policy);

            // basicamente ele tenta ver se acha a politica necessaria, se não existir ele
            // tenta criar usando a política de requiremento de permissões criada
            return await Task.FromResult(new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName)).Build());
        }

        public async  Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _fallbackProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await _fallbackProvider.GetFallbackPolicyAsync();
        }
    }
}