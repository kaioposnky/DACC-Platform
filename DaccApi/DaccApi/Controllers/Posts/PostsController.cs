using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Services.Posts;



namespace DaccApi.Controllers.Posts
{
    [ApiController]
    [Route("api/posts")]

    public class PostsController : ControllerBase
    {
        private readonly IPostsServices _postsServices;

        public PostsController(IPostsServices postsServices)
        {
            _postsServices = postsServices;
        }

        [HttpGet("")]
        public IActionResult GetAllPosts()
        {
            var response = _postsServices.GetAllPosts();
            return response;
        }
        
        [HttpPost("")]
        public IActionResult CreatePost([FromBody] RequestPost request)
        {
            var response = _postsServices.CreatePost(request);
            return response;
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            var response = _postsServices.GetPostById(id);
            return response;
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePost([FromRoute] int id)
        {

            var response = _postsServices.DeletePost(id);
            return response;
        }
        
        [HttpPatch("{id:int}")]
        public IActionResult UpdatePost([FromRoute] int id, [FromBody] RequestPost request)
        {
            var response = _postsServices.UpdatePost(id, request);
            return response;
        }

        /* IMPLEMENTAR PARTE DE VOTAÇÃO DO POST
        [HttpPost("{id:int}"/vote)]
        public IActionResult VotePost([FromRoute] int id)
        {
            return Ok();
        }
        */
        
    }
    
}