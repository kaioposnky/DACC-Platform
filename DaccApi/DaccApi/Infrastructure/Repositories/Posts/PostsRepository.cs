using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Post;
using DaccApi.Model;


namespace DaccApi.Infrastructure.Repositories.Posts

{
    public class PostsRepository : IPostsRepository
{
    private readonly IRepositoryDapper _repositoryDapper;

    public PostsRepository(IRepositoryDapper repositoryDapper)
    {
        _repositoryDapper = repositoryDapper;
    }

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
            throw new Exception("Erro ao obter todas os Posts no banco de dados.");
        }
  
    }

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
            throw new Exception("Erro ao criar post.");
        }
    }

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
            throw new Exception("Erro ao deletar post.");
        }
    }
    
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
            throw new Exception("Erro ao obter post especificado no banco de dados.");
        }
    }
    
    
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
            throw new Exception("Erro ao atualizar post no banco de dados.");
        };
    }
}
    
    
}

