using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Infrastructure.Authentication{
    /// <summary>
    /// Representa um requisito de autorização baseado em uma permissão específica.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Obtém ou define a permissão necessária.
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissionRequirement"/>.
        /// </summary>
        /// <param name="permission">A permissão necessária.</param>
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}