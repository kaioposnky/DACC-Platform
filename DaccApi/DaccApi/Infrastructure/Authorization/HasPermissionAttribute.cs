using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) : base(policy: permission)
        {
            
        }
    }
}