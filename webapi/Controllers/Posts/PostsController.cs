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
        public async Task<IActionResult> GetPosts()
        {
            var response = await _postsServices.GetAllPosts();
            return response;
        }

        /// <summary>
        /// Cria um novo post.
        /// </summary>
        [HttpPost("")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public async Task<IActionResult> CreatePost([FromBody] RequestPost request)
        {
            var response = await _postsServices.CreatePost(request);
            return response;
        }

        /// <summary>
        /// Obtém um post específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid id)
        {
            var response = await _postsServices.GetPostById(id);
            return response;
        }

        /// <summary>
        /// Deleta um post existente.
        /// </summary>
        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid id)
        {
            var response = await _postsServices.DeletePost(id);
            return response;
        }

        /// <summary>
        /// Atualiza um post existente.
        /// </summary>
        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, [FromBody] RequestPost request)
        {
            var response = await _postsServices.UpdatePost(id, request);
            return response;
        }
    }
}