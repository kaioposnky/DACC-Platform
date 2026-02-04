using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Model;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

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

    public async Task<(List<Noticia> Noticias, int TotalCount)> GetAllNoticias(RequestQueryNoticia request)
    {
        var result = await _noticiasRepository.SearchNoticias(request);

        return result;
    }

    public async Task<IActionResult> CreateNoticia(Guid autorId, RequestNoticia request)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(request.Title) ||
                String.IsNullOrWhiteSpace(request.Category) ||
                String.IsNullOrWhiteSpace(request.Description))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            var noticia = new Noticia()
            {
                Id = Guid.NewGuid(),
                Categoria = request.Category?.ToLower() ?? string.Empty,
                Descricao = request.Description,
                Conteudo = request.Content,
                Titulo = request.Title,
                AutorId = autorId,
                TempoLeitura = request.ReadTime,
                ImagemUrl = request.ImageUrl,
                ImagemAlt = request.ImageAlt,
                DataPublicacao = request.PublishedAt ?? DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };
            
            await _noticiasRepository.CreateAsync(noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> UpdateNoticiaImage(Guid noticiaId, ImageRequest request)
    {
        try
        {
            var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);

            var noticia = await _noticiasRepository.GetByIdAsync(noticiaId);

            if (noticia == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Notícia não encontrada!");
            }
            
            noticia.ImagemUrl = imageUrl;
            noticia.ImagemAlt = request.ImageAlt;
            
            await _noticiasRepository.UpdateAsync(noticiaId, noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
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
            var noticia = await _noticiasRepository.GetByIdAsync(id);
            
            if (noticia == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            await _noticiasRepository.DeleteAsync(id);

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
            var noticia =  await _noticiasRepository.GetByIdAsync(id);

            if (noticia == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Noticia>()));

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new { news = new ResponseNoticia(noticia) }));
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
            var noticiaQuery = await _noticiasRepository.GetByIdAsync(id);
            if (noticiaQuery == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            noticiaQuery.Titulo = request.Title ?? noticiaQuery.Titulo;
            noticiaQuery.Descricao = request.Description ?? noticiaQuery.Descricao;
            noticiaQuery.Categoria = request.Category ?? noticiaQuery.Categoria;
            noticiaQuery.Conteudo = request.Content ?? noticiaQuery.Conteudo;
            noticiaQuery.TempoLeitura = request.ReadTime ?? noticiaQuery.TempoLeitura;
            noticiaQuery.ImagemUrl = request.ImageUrl ?? noticiaQuery.ImagemUrl;
            noticiaQuery.ImagemAlt = request.ImageAlt ?? noticiaQuery.ImagemAlt;
            noticiaQuery.DataPublicacao = request.PublishedAt ?? noticiaQuery.DataPublicacao;
            noticiaQuery.DataAtualizacao = DateTime.UtcNow;
            
            await _noticiasRepository.UpdateAsync(id, noticiaQuery);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
 }
}