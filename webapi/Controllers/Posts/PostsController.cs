using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Posts;

namespace DaccApi.Controllers.Posts
{
    /// <summary>
    /// Controlador para gerenciar posts do fórum (atualmente não implementado).
    /// </summary>
    [Authorize]
    [ApiController]
    [NonController]
    [Route("v1/api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsServices _postsServices;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PostsController"/>.
        /// </summary>
        public PostsController(IPostsServices postsServices)
        {
            _postsServices = postsServices;
        }

        /// <summary>
        /// Obtém todos os posts.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetPosts()
        {
            var response = _postsServices.GetAllPosts();
            return response;
        }

        /// <summary>
        /// Cria um novo post.
        /// </summary>
        [HttpPost("")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public IActionResult CreatePost([FromBody] RequestPost request)
        {
            var response = _postsServices.CreatePost(request);
            return response;
        }

        /// <summary>
        /// Obtém um post específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            var response = _postsServices.GetPostById(id);
            return response;
        }

        /// <summary>
        /// Deleta um post existente.
        /// </summary>
        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public IActionResult DeletePost([FromRoute] int id)
        {
            var response = _postsServices.DeletePost(id);
            return response;
        }

        /// <summary>
        /// Atualiza um post existente.
        /// </summary>
        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public IActionResult UpdatePost([FromRoute] int id, [FromBody] RequestPost request)
        {
            var response = _postsServices.UpdatePost(id, request);
            return response;
        }
    }
}