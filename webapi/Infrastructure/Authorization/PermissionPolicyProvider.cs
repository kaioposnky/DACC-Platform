using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DaccApi.Infrastructure.Authentication
{
    /// <summary>
    /// Provedor de políticas de autorização que cria políticas baseadas em permissões dinamicamente.
    /// </summary>
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissionPolicyProvider"/>.
        /// </summary>
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        
        /// <summary>
        /// Obtém uma política de autorização para o nome de política fornecido.
        /// </summary>
        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await _fallbackProvider.GetPolicyAsync(policyName);
            if (policy != null) return await Task.FromResult(policy);

            // basicamente ele tenta ver se acha a politica necessaria, se não existir ele
            // tenta criar usando a política de requiremento de permissões criada
            return await Task.FromResult(new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName)).Build());
        }

        /// <summary>
        /// Obtém a política de autorização padrão.
        /// </summary>
        public async  Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _fallbackProvider.GetDefaultPolicyAsync();
        }

        /// <summary>
        /// Obtém a política de autorização de fallback.
        /// </summary>
        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await _fallbackProvider.GetFallbackPolicyAsync();
        }
    }
}