using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Forum
{
    [Authorize]
    [ApiController]
    [NonController]
    [Route("v1/api/[controller]")]
    public class ForumController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetForumCategories()
        { 
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetForumCategory(int id)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("threads")]
        public IActionResult GetForumThreads()
        {
            throw new NotImplementedException();
        }

        [HttpGet("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.ViewPosts)]
        public IActionResult GetForumThread(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("threads")]
        [HasPermission(AppPermissions.Forum.CreatePost)]
        public IActionResult CreateForumThread()
        {
            throw new NotImplementedException();
        }

        [HttpPut("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.UpdateOwnPost)]
        public IActionResult UpdateForumThread(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("threads/{id:int}")]
        [HasPermission(AppPermissions.Forum.DeleteOwnPost)]
        public IActionResult DeleteForumThread(int id)
        {
            throw new NotImplementedException();
        }
    }
}