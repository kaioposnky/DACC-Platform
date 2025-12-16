using DaccApi.Model;
using DaccApi.Model.Post;


namespace DaccApi.Infrastructure.Repositories.Posts

{
    /// <summary>
    /// Define a interface para o repositório de posts.
    /// </summary>
    public interface IPostsRepository
    {
        /// <summary>
        /// Obtém todos os posts.
        /// </summary>
        public Task<List<Post>> GetAllPosts();
    
        /// <summary>
        /// Cria um novo post.
        /// </summary>
        public Task CreatePost(RequestPost post);

        /// <summary>
        /// Deleta um post existente.
        /// </summary>
        public Task DeletePost(int id);
    
        /// <summary>
        /// Obtém um post específico pelo seu ID.
        /// </summary>
        public Task<Post?> GetPostById(int id);

        /// <summary>
        /// Atualiza um post existente.
        /// </summary>
        public Task UpdatePost(int id, RequestPost post);
    }
}
