using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Posts
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetPosts()
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public IActionResult CreatePost()
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public IActionResult DeletePost([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public IActionResult UpdatePost([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}