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
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                // Mapeia os anuncios para responses
                var anunciosResponse = anuncios.Select(anuncio => new ResponseAnuncio(anuncio));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { anuncios = anunciosResponse }));
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
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                var anuncioResponse = new ResponseAnuncio(anuncio);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { anuncio = anuncioResponse }));
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
                    String.IsNullOrEmpty(anuncio.Titulo) ||
                    String.IsNullOrEmpty(anuncio.Conteudo)
                )
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var entity = new Anuncio
                {
                    Id = Guid.NewGuid(),
                    Titulo = anuncio.Titulo,
                    Conteudo = anuncio.Conteudo,
                    TipoAnuncio = anuncio.TipoAnuncio,
                    Ativo = anuncio.Ativo,
                    AutorId = autorId,
                    BotaoPrimarioTexto = anuncio.BotaoPrimarioTexto ?? string.Empty,
                    BotaoPrimarioLink = anuncio.BotaoPrimarioLink ?? string.Empty,
                    BotaoSecundarioTexto = anuncio.BotaoSecundarioTexto ?? string.Empty,
                    BotaoSecundarioLink = anuncio.BotaoSecundarioLink ?? string.Empty,
                    ImagemUrl = anuncio.ImagemUrl ?? string.Empty,
                    ImagemAlt = anuncio.ImagemAlt ?? string.Empty,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };
                await _anuncioRepository.CreateAsync(entity);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
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

                anuncioQuery.Titulo = request.Titulo;
                anuncioQuery.Conteudo = request.Conteudo;
                anuncioQuery.TipoAnuncio = request.TipoAnuncio;
                anuncioQuery.Ativo = request.Ativo;
                anuncioQuery.BotaoPrimarioTexto = request.BotaoPrimarioTexto ?? anuncioQuery.BotaoPrimarioTexto;
                anuncioQuery.BotaoPrimarioLink = request.BotaoPrimarioLink ?? anuncioQuery.BotaoPrimarioLink;
                anuncioQuery.BotaoSecundarioTexto = request.BotaoSecundarioTexto ?? anuncioQuery.BotaoSecundarioTexto;
                anuncioQuery.BotaoSecundarioLink = request.BotaoSecundarioLink ?? anuncioQuery.BotaoSecundarioLink;
                anuncioQuery.ImagemUrl = request.ImagemUrl ?? anuncioQuery.ImagemUrl;
                anuncioQuery.ImagemAlt = request.ImagemAlt ?? anuncioQuery.ImagemAlt;
                anuncioQuery.DataAtualizacao = DateTime.UtcNow;
                
                await _anuncioRepository.UpdateAsync(id, anuncioQuery);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                    new { anuncio = request }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
