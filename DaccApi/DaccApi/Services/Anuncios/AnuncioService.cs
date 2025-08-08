using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Anuncios
{
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
                var anuncios = await _anuncioRepository.GetAllAnuncio();
                if (anuncios.Count == 0) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { anuncios = anuncios}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
            
        }
        
        public async Task<IActionResult> GetAnuncioById(Guid id)
        {
            try
            {
                var anuncio = await _anuncioRepository.GetAnuncioById(id);
                if (anuncio == null) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { anuncio = anuncio}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
            
        }

        public async Task<IActionResult> CreateAnuncio(RequestAnuncio anuncio)
        {
            try
            {
                if (
                    String.IsNullOrEmpty(anuncio.Titulo) ||
                    String.IsNullOrEmpty(anuncio.Conteudo) ||
                    String.IsNullOrEmpty(anuncio.ImagemAlt)||
                    String.IsNullOrEmpty(anuncio.ImagemAlt)||
                    String.IsNullOrEmpty(anuncio.ImagemAlt)
                    )
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
            
                _anuncioRepository.CreateAnuncio(anuncio);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        public async Task<IActionResult> DeleteAnuncio(Guid id)
        {

            try
            {
                var anuncio = await _anuncioRepository.GetAnuncioById(id);
            
                if (anuncio == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                _anuncioRepository.DeleteAnuncio(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        // TODO: Substituir RequestAnuncio por DTO para atualização de anuncio
        public async Task<IActionResult> UpdateAnuncio(Guid id, RequestAnuncio request)
        {
            try
            {
                var anuncioQuery = await _anuncioRepository.GetAnuncioById(id);
                if (anuncioQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Anúncio não encontrado!");
                }

                var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile!);
                
                var anuncio = new Anuncio()
                {
                   Titulo = request.Titulo,
                   Conteudo = request.Conteudo,
                   ImagemAlt = request.ImagemAlt,
                   ImagemUrl = imageUrl,
                   TipoAnuncio = request.TipoAnuncio,
                   Ativo = request.Ativo
                };
                await _anuncioRepository.UpdateAnuncio(id, anuncio);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { anuncio = request}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        
        
    }
    
}
