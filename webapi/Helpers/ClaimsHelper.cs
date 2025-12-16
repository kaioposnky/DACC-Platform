using System.Security.Claims;

namespace DaccApi.Helpers
{
    /// <summary>
    /// Classe auxiliar para manipulação de claims de usuário.
    /// </summary>
    public class ClaimsHelper
    {
        /// <summary>
        /// Obtém o ID do usuário a partir do ClaimsPrincipal.
        /// </summary>
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}