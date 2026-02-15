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
                String.IsNullOrWhiteSpace(request.CategoryName) ||
                String.IsNullOrWhiteSpace(request.Description))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            var noticia = new Noticia()
            {
                Id = Guid.NewGuid(),
                Categoria = request.CategoryName?.ToLower() ?? string.Empty,
                Descricao = request.Description,
                Conteudo = request.Content,
                Titulo = request.Title,
                AutorId = request.AuthorId ?? autorId,
                TempoLeitura = request.ReadTime,
                ImagemUrl = request.Image,
                ImagemAlt = request.ImageAlt,
                DataPublicacao = request.Date ?? DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };
            
            await _noticiasRepository.CreateAsync(noticia);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { news = new ResponseNoticia(noticia)}));
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
            string imageUrl;
            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                {
                    imageUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                }
                else
                {
                    imageUrl = request.ImageUrl;
                }
            }
            else
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "A imagem é obrigatória.");
            }

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
            noticiaQuery.Categoria = request.CategoryName ?? noticiaQuery.Categoria;
            noticiaQuery.Conteudo = request.Content ?? noticiaQuery.Conteudo;
            noticiaQuery.TempoLeitura = request.ReadTime ?? noticiaQuery.TempoLeitura;
            if (request.Image != null)
            {
                if (request.Image.StartsWith("data:image") || request.Image.Length > 255)
                {
                    noticiaQuery.ImagemUrl = await _fileStorageService.SaveBase64ImageAsync(request.Image);
                }
                else
                {
                    noticiaQuery.ImagemUrl = request.Image;
                }
            }
            noticiaQuery.ImagemAlt = request.ImageAlt ?? noticiaQuery.ImagemAlt;
            noticiaQuery.DataPublicacao = request.Date ?? noticiaQuery.DataPublicacao;
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