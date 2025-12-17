using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Posts
{
    
    public interface IPostsServices
    {
        public Task<IActionResult> CreatePost(RequestPost post);
    
        public Task<IActionResult> GetAllPosts();
        public Task<IActionResult> UpdatePost(int id, RequestPost post);
    
        public Task<IActionResult> GetPostById(int id);
        public Task<IActionResult> DeletePost(int id);
        public Task<IActionResult> VotePosts();
    }
    
    
}

