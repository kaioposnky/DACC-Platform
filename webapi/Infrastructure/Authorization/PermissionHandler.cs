using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Infrastructure.Authentication
{
    /// <summary>
    /// Manipulador de autorização que verifica se o usuário possui a permissão necessária.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Manipula a verificação de um requisito de permissão.
        /// </summary>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim("permission", requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}