using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Posts;

namespace DaccApi.Controllers.Posts
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsServices _postsServices;
        public PostsController(IPostsServices postsServices)
        {
            _postsServices = postsServices;
        }
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetPosts()
        {
            var response = _postsServices.GetAllPosts();
            return response;
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public IActionResult CreatePost([FromBody] RequestPost request)
        {
            var response = _postsServices.CreatePost(request);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            var response = _postsServices.GetPostById(id);
            return response;
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public IActionResult DeletePost([FromRoute] int id)
        {
            var response = _postsServices.DeletePost(id);
            return response;
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public IActionResult UpdatePost([FromRoute] int id, [FromBody] RequestPost request)
        {
            var response = _postsServices.UpdatePost(id, request);
            return response;
        }
    }
}