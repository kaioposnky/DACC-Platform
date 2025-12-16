 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using System;

namespace DaccApi.Controllers.Comentarios
{
    /// <summary>
    /// Controlador para gerenciar comentários (atualmente não implementado).
    /// </summary>
    [Authorize]
    [ApiController]
    [NonController]
    [Route("v1/api/")]
    public class ComentariosController : ControllerBase
    {
        /// <summary>
        /// Obtém os comentários de um post específico.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("posts/{postId:int}/comments")]
        public IActionResult GetComentariosPost([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Cria um novo comentário em um post.
        /// </summary>
        [HttpPost("posts/comments/{postId:int}/comments")]
        [HasPermission(AppPermissions.Forum.CreateComment)]
        public IActionResult CreateComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Obtém um comentário específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("comments/{id:int}")]
        public IActionResult GetComentarioById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deleta um comentário existente.
        /// </summary>
        [HttpDelete("comments/{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnComment)]
        public IActionResult DeleteComentario([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atualiza um comentário existente.
        /// </summary>
        [HttpPatch("comments/{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnComment)]
        public IActionResult UpdateComentario([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Vota em um comentário.
        /// </summary>
        [HttpPost("comments/{postId:int}/vote")]
        [HasPermission(AppPermissions.Forum.VoteOnComment)]
        public IActionResult VoteComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Aceita um comentário como resposta.
        /// </summary>
        [HttpPut("comments/{postId:int}/accept")]
        [HasPermission(AppPermissions.Forum.AcceptComment)]
        public IActionResult AcceptComentario([FromRoute] int postId)
        {
            throw new NotImplementedException();
        }
    }
}