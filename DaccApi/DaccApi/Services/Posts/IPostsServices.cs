using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Posts
{
    
    public interface IPostsServices
    {
        public IActionResult CreatePost(RequestPost post);
    
        public IActionResult GetAllPosts();
        public IActionResult UpdatePost(int id, RequestPost post);
    
        public IActionResult GetPostById(int id);
        public IActionResult DeletePost(int id);
        public IActionResult VotePosts();
    }
    
    
}

