using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Infrastructure.Authentication
{
    /// <summary>
    /// Atributo para autorização baseada em permissões.
    /// </summary>
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="HasPermissionAttribute"/>.
        /// </summary>
        /// <param name="permission">A permissão necessária para acessar o recurso.</param>
        public HasPermissionAttribute(string permission) : base(policy: permission)
        {
            
        }
    }
}