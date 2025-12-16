using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Post;
using DaccApi.Model;


namespace DaccApi.Infrastructure.Repositories.Posts

{
    /// <summary>
    /// Implementação do repositório de posts.
    /// </summary>
    public class PostsRepository : IPostsRepository
{
    private readonly IRepositoryDapper _repositoryDapper;

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="PostsRepository"/>.
    /// </summary>
    public PostsRepository(IRepositoryDapper repositoryDapper)
    {
        _repositoryDapper = repositoryDapper;
    }

    /// <summary>
    /// Obtém todos os posts.
    /// </summary>
    public async Task<List<Post>> GetAllPosts()
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllPosts");
            var queryResult = await _repositoryDapper.QueryAsync<Post>(sql);
            var post = queryResult.ToList();
            return post;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas os Posts no banco de dados." + ex.Message);
        }
  
    }

    /// <summary>
    /// Cria um novo post.
    /// </summary>
    public async Task CreatePost(RequestPost post)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("CreatePost");
            
            var param = new
            {
                Titulo = post.Titulo,
                Conteudo = post.Conteudo,
                Tags = post.Tags,
            };

            await _repositoryDapper.ExecuteAsync(sql, param);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar post." + ex.Message);
        }
    }

    /// <summary>
    /// Deleta um post existente.
    /// </summary>
    public async Task DeletePost(int id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("DeletePost");
            var param = new { id = id };
            await _repositoryDapper.ExecuteAsync(sql, param);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao deletar post." + ex.Message);
        }
    }
    
    /// <summary>
    /// Obtém um post específico pelo seu ID.
    /// </summary>
    public async Task<Post?> GetPostById(int id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetPostById");

            var queryResult = await _repositoryDapper.QueryAsync<Post>(sql);

            var posts = queryResult.FirstOrDefault();
            return posts;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter post especificado no banco de dados." + ex.Message);
        }
    }
    
    
    /// <summary>
    /// Atualiza um post existente.
    /// </summary>
    public async Task UpdatePost(int id, RequestPost post)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("UpdatePosts");
            var param = new
            {
                id = id,
                Titulo = post.Titulo,
                Conteudo = post.Conteudo,
                Tags = post.Tags,
            };
            await _repositoryDapper.ExecuteAsync(sql, param);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar post no banco de dados." + ex.Message);
        };
    }
}
    
    
}
