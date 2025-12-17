using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Posts;
using DaccApi.Model;
using DaccApi.Model.Responses.Post;
using DaccApi.Responses;
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

    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _postsRepository.GetAllPosts();

            if (posts.Count == 0)
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            var response = posts.Select(post => new ResponsePost(post));
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new {posts = response}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
    
    public async Task<IActionResult> CreatePost(RequestPost post)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(post.Titulo) ||
                String.IsNullOrWhiteSpace(post.Conteudo) ||
                post.Tags == null || post .Tags.Length == 0)
                
            {
                // TODO: Adicionar campo de detalhes no request
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            await _postsRepository.CreatePost(post);
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var posts = await _postsRepository.GetPostById(id);
            
            if (posts == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            await _postsRepository.DeletePost(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> GetPostById(int id)
    {
        try
        {
            var post = await _postsRepository.GetPostById(id);

            if (post == null) 
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            var response = new ResponsePost(post);
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new { post = response}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
    
    
    public async Task<IActionResult> VotePosts()
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> UpdatePost(int id, RequestPost postUpdated)
    {
        try
        {
            var post = await _postsRepository.GetPostById(id);
            if (post == null)
            {
                ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            else if (string.IsNullOrWhiteSpace(postUpdated.Conteudo) ||
                postUpdated.Tags.Length == 0)
            {
                // TODO: Adicionar campo de detalhes no request
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            await _postsRepository.UpdatePost(id, postUpdated);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { posts = postUpdated}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    
}
}


