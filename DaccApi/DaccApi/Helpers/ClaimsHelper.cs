using System.Security.Claims;

namespace DaccApi.Helpers
{
    public class ClaimsHelper
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}