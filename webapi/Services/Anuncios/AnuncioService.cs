using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Model;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Anuncios
{
    /// <summary>
    /// Classe que gerencia serviços dos anúncios
    /// </summary>
    public class AnuncioService : IAnuncioService
    {
        private readonly IAnuncioRepository _anuncioRepository;
        private readonly IFileStorageService _fileStorageService;

        public AnuncioService(IAnuncioRepository anuncioRepository, IFileStorageService fileStorageService)
        {
            _anuncioRepository = anuncioRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> GetAllAnuncio()
        {
            try
            {
                var anuncios = await _anuncioRepository.GetAllAsync();
                if (anuncios.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Anuncio>()));

                // Mapeia os anuncios para responses
                var anunciosResponse = anuncios.Select(anuncio => new ResponseAnuncio(anuncio));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { announcements = anunciosResponse }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetAnuncioById(Guid id)
        {
            try
            {
                var anuncio = await _anuncioRepository.GetByIdAsync(id);
                if (anuncio == null)
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Anúncio não encontrado!");
                var anuncioResponse = new ResponseAnuncio(anuncio);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { announcement = anuncioResponse }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateAnuncio(RequestAnuncio anuncio, Guid autorId)
        {
            try
            {
                if (
                    string.IsNullOrEmpty(anuncio.Title) ||
                    string.IsNullOrEmpty(anuncio.Content) ||
                    string.IsNullOrEmpty(anuncio.Type)
                )
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var entity = new Anuncio
                {
                    Id = Guid.NewGuid(),
                    Titulo = anuncio.Title,
                    Conteudo = anuncio.Content,
                    TipoAnuncio = anuncio.Type,
                    Ativo = anuncio.IsActive,
                    AutorId = autorId,
                    BotaoPrimarioTexto = anuncio.PrimaryButtonText ?? string.Empty,
                    BotaoPrimarioLink = anuncio.PrimaryButtonLink ?? string.Empty,
                    BotaoSecundarioTexto = anuncio.SecondaryButtonText ?? string.Empty,
                    BotaoSecundarioLink = anuncio.SecondaryButtonLink ?? string.Empty,
                    ImagemUrl = anuncio.ImageUrl ?? string.Empty,
                    ImagemAlt = anuncio.ImageAlt ?? string.Empty,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };
                await _anuncioRepository.CreateAsync(entity);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseAnuncio(entity)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> AddAnuncioImage(Guid id, ImageRequest request)
        {
            try
            {
                var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);

                var anuncio = await _anuncioRepository.GetByIdAsync(id);

                if (anuncio == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Notícia não encontrada!");
                }

                anuncio.ImagemUrl = imageUrl;
                anuncio.ImagemAlt = request.ImageAlt;

                await _anuncioRepository.UpdateAsync(id, anuncio);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(request));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    "Erro ao adicionar anuncio na notícia." + ex.Message);
            }
        }


        public async Task<IActionResult> DeleteAnuncio(Guid id)
        {
            try
            {
                var anuncio = await _anuncioRepository.GetByIdAsync(id);

                if (anuncio == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }

                await _anuncioRepository.DeleteAsync(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        // TODO: Substituir RequestAnuncio por DTO para atualização de anuncio
        public async Task<IActionResult> UpdateAnuncio(Guid id, RequestAnuncio request)
        {
            try
            {
                var anuncioQuery = await _anuncioRepository.GetByIdAsync(id);
                if (anuncioQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND,
                        "Anúncio não encontrado!");
                }

                anuncioQuery.Titulo = request.Title ?? anuncioQuery.Titulo;
                anuncioQuery.Conteudo = request.Content ?? anuncioQuery.Conteudo;
                anuncioQuery.TipoAnuncio = request.Type ?? anuncioQuery.TipoAnuncio;
                anuncioQuery.Ativo = request.IsActive;
                anuncioQuery.BotaoPrimarioTexto = request.PrimaryButtonText ?? anuncioQuery.BotaoPrimarioTexto;
                anuncioQuery.BotaoPrimarioLink = request.PrimaryButtonLink ?? anuncioQuery.BotaoPrimarioLink;
                anuncioQuery.BotaoSecundarioTexto = request.SecondaryButtonText ?? anuncioQuery.BotaoSecundarioTexto;
                anuncioQuery.BotaoSecundarioLink = request.SecondaryButtonLink ?? anuncioQuery.BotaoSecundarioLink;
                anuncioQuery.ImagemUrl = request.ImageUrl ?? anuncioQuery.ImagemUrl;
                anuncioQuery.ImagemAlt = request.ImageAlt ?? anuncioQuery.ImagemAlt;
                anuncioQuery.DataAtualizacao = DateTime.UtcNow;
                
                await _anuncioRepository.UpdateAsync(id, anuncioQuery);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { announcement = request }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }


        public async Task<IActionResult> SearchAnuncio(RequestQueryAnuncio query)
        {
            try
            {
                var (anuncios, totalCount) = await _anuncioRepository.SearchAnuncio(query);
                if (anuncios.Count == 0 && totalCount == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Anuncio>()));

                // Mapeia os anuncios para responses
                var anunciosResponse = anuncios.Select(anuncio => new ResponseAnuncio(anuncio));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { announcements = anunciosResponse, totalCount = totalCount }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
