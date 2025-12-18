using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Post;

namespace DaccApi.Infrastructure.Repositories.Posts;

public class PostsRepository : BaseRepository<Post>, IPostsRepository
{
    public PostsRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
    {
    }
}