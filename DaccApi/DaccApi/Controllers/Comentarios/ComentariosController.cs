using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Comentarios
{
    [Authorize]
    [ApiController]
    [NonController]
    [Route("api/")]
    public class ComentariosController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("posts/{postId:int}/comments")]
        public IActionResult GetComentariosPost([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("posts/comments/{postId:int}/comments")]
        [HasPermission(AppPermissions.Forum.CreateComment)]
        public IActionResult CreateComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        [AllowAnonymous]
        [HttpGet("comments/{id:int}")]
        public IActionResult GetComentarioById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("comments/{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnComment)]
        public IActionResult DeleteComentario([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("comments/{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnComment)]
        public IActionResult UpdateComentario([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("comments/{postId:int}/vote")]
        [HasPermission(AppPermissions.Forum.VoteOnComment)]
        public IActionResult VoteComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("comments/{postId:int}/accept")]
        [HasPermission(AppPermissions.Forum.AcceptComment)]
        public IActionResult AcceptComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
    }
}