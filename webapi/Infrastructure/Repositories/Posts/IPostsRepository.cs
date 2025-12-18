using DaccApi.Model.Post;

namespace DaccApi.Infrastructure.Repositories.Posts
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Post entity);
        Task<bool> UpdateAsync(Guid id, Post entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
