using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias
{
    
public class NoticiasServices : INoticiasServices
{
    private readonly INoticiasRepository _noticiasRepository;

    public NoticiasServices(INoticiasRepository noticiasRepository)
    {
        _noticiasRepository = noticiasRepository;
    }

    public async Task<IActionResult> GetAllNoticias()
    {
        try
        {
            var noticias =  await _noticiasRepository.GetAllNoticias();

            if (noticias.Count == 0) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticias}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> CreateNoticia(RequestNoticia noticia)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(noticia.Titulo) ||
                String.IsNullOrWhiteSpace(noticia.Categoria) ||
                String.IsNullOrWhiteSpace(noticia.Descricao))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            _noticiasRepository.CreateNoticia(noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> DeleteNoticia(int id)
    {

        try
        {
            var noticia = await _noticiasRepository.GetNoticiaById(id);
            
            if (noticia == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            _noticiasRepository.DeleteNoticia(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }


    public async Task<IActionResult> GetNoticiaById(int id)
    {
        try
        {
            var noticia =  await _noticiasRepository.GetNoticiaById(id);

            if (noticia == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticia}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> UpdateNoticia(int id,RequestNoticia noticia)
    {
        try
        {
            var noticiaQuery = await _noticiasRepository.GetNoticiaById(id);
            if (noticiaQuery == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            await _noticiasRepository.UpdateNoticia(id, noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticia}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
 }
}