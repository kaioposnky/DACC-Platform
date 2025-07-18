using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Posts;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Posts
{
    public class PostsServices : IPostsServices
{
    private readonly IPostsRepository _postsRepository;

    
    public PostsServices(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public IActionResult GetAllPosts()
    {
        try
        {
            var posts = _postsRepository.GetAllPosts().Result;

            if (posts.Count == 0)
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new {posts = posts}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }
    
    public IActionResult CreatePost(RequestPost post)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(post.Titulo) ||
                String.IsNullOrWhiteSpace(post.Conteudo) ||
                post.Tags == null || post .Tags.Length == 0)
                
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            _postsRepository.CreatePost(post);
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    public IActionResult DeletePost(int id)
    {
        try
        {
            var posts = _postsRepository.GetPostById(id).Result;
            
            if (posts == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            _postsRepository.DeletePost(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    public IActionResult GetPostById(int id)
    {
        try
        {
            var post = _postsRepository.GetPostById(id).Result;

            if (post == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { posts = post}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }
    
    
    public IActionResult VotePosts()
    {
        throw new NotImplementedException();
    }

    public IActionResult UpdatePost(int id, RequestPost post)
    {
        try
        {
            var postQuery = _postsRepository.GetPostById(id).Result;
            if (postQuery == null ||
                String.IsNullOrWhiteSpace(post.Conteudo) ||
                post.Tags == null || post.Tags.Length == 0)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            _postsRepository.UpdatePost(id, post);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { posts = post}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    
}
}


