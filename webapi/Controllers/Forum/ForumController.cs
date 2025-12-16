using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Forum
{
    /// <summary>
    /// Controlador para gerenciar o fórum (atualmente não implementado).
    /// </summary>
    [Authorize]
    [ApiController]
    [NonController]
    [Route("v1/api/[controller]")]
    public class ForumController : ControllerBase
    {
        /// <summary>
        /// Obtém todas as categorias do fórum.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetForumCategories()
        { 
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém uma categoria específica do fórum pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetForumCategory(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém todos os tópicos do fórum.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("threads")]
        public IActionResult GetForumThreads()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um tópico específico do fórum pelo seu ID.
        /// </summary>
        [HttpGet("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.ViewPosts)]
        public IActionResult GetForumThread(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cria um novo tópico no fórum.
        /// </summary>
        [HttpPost("threads")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public IActionResult CreateForumThread()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atualiza um tópico existente no fórum.
        /// </summary>
        [HttpPut("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public IActionResult UpdateForumThread(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deleta um tópico existente no fórum.
        /// </summary>
        [HttpDelete("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public IActionResult DeleteForumThread(int id)
        {
            throw new NotImplementedException();
        }
    }
}