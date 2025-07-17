using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public class NoticiasServices : INoticiasServices
{
    private readonly INoticiasRepository _noticiasRepository;

    public NoticiasServices(INoticiasRepository noticiasRepository)
    {
        _noticiasRepository = noticiasRepository;
    }

    public IActionResult GetAllNoticias()
    {
        try
        {
            var noticias = _noticiasRepository.GetAllNoticias().Result;

            if (noticias.Count == 0) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticias}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    public IActionResult CreateNoticia(RequestNoticia noticia)
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
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    public IActionResult DeleteNoticia(int id)
    {

        try
        {
            var noticia = _noticiasRepository.GetNoticiaById(id).Result;
            
            if (noticia == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            _noticiasRepository.DeleteNoticia(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }


    public IActionResult GetNoticiaById(int id)
    {
        try
        {
            var noticia = _noticiasRepository.GetNoticiaById(id).Result;

            if (noticia == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticia}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }

    public IActionResult UpdateNoticia(int id,RequestNoticia noticia)
    {
        try
        {
            var noticiaQuery = _noticiasRepository.GetNoticiaById(id).Result;
            if (noticiaQuery == null ||
                String.IsNullOrWhiteSpace(noticia.Categoria) ||
                String.IsNullOrWhiteSpace(noticia.Descricao))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            _noticiasRepository.UpdateNoticia(id, noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = noticia}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
        }
    }
    
}