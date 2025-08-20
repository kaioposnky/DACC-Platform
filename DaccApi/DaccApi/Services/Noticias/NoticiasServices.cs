using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Model;
using DaccApi.Model.Requests;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias
{
    
public class NoticiasServices : INoticiasServices
{
    private readonly INoticiasRepository _noticiasRepository;
    private readonly IFileStorageService _fileStorageService;   

    public NoticiasServices(INoticiasRepository noticiasRepository, IFileStorageService fileStorageService)
    {
        _noticiasRepository = noticiasRepository;
        _fileStorageService = fileStorageService;   
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

    public async Task<IActionResult> CreateNoticia(Guid autorId, RequestNoticia request)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(request.Titulo) ||
                String.IsNullOrWhiteSpace(request.Categoria) ||
                String.IsNullOrWhiteSpace(request.Descricao))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            var noticia = new Noticia()
            {
                Categoria = request.Categoria,
                Descricao = request.Descricao,
                Titulo = request.Titulo,
                AutorId = autorId,
            };
            
            await _noticiasRepository.CreateNoticia(noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> AddNoticiaImage(Guid noticiaId, ImageRequest request)
    {
        try
        {
            var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);

            var noticia = await _noticiasRepository.GetNoticiaById(noticiaId);

            if (noticia == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Notícia não encontrada!");
            }
            
            noticia.ImagemUrl = imageUrl;
            noticia.ImagemAlt = request.ImageAlt;
            
            await _noticiasRepository.UpdateNoticia(noticiaId, noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(noticia));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao adicionar imagem na notícia." + ex.Message);
        }
    }
    
    public async Task<IActionResult> DeleteNoticia(Guid id)
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


    public async Task<IActionResult> GetNoticiaById(Guid id)
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

    public async Task<IActionResult> UpdateNoticia(Guid id,RequestNoticia request)
    {
        try
        {
            var noticiaQuery = await _noticiasRepository.GetNoticiaById(id);
            if (noticiaQuery == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            var noticia = new Noticia()
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                Categoria = request.Categoria,
            };
            
            await _noticiasRepository.UpdateNoticia(id, noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { noticias = request}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
 }
}