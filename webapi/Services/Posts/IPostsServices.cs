using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Posts
{
    
    public interface IPostsServices
    {
        public Task<IActionResult> CreatePost(RequestPost post);
    
        public Task<IActionResult> GetAllPosts();
        public Task<IActionResult> UpdatePost(Guid id, RequestPost post);
    
        public Task<IActionResult> GetPostById(Guid id);
        public Task<IActionResult> DeletePost(Guid id);
        public Task<IActionResult> VotePosts();
    }
    
    
}
