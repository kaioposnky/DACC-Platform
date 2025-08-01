using DaccApi.Model;
using DaccApi.Model.Post;


namespace DaccApi.Infrastructure.Repositories.Posts

{
    
    public interface IPostsRepository
    {
        public Task<List<Post>> GetAllPosts();
    
        public Task CreatePost(RequestPost post);

        public Task DeletePost(int id);
    
        public Task<Post?> GetPostById(int id);

        public Task UpdatePost(int id, RequestPost post);
    }
}

