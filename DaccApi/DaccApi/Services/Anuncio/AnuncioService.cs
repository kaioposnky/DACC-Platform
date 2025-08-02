


using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Anuncio
{
    public class AnuncioService : IAnuncioService
    {
        private readonly IAnuncioRepository _anuncioRepository;

        public AnuncioService(IAnuncioRepository anuncioRepository)
        {
            _anuncioRepository = anuncioRepository;
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
        
        
        public async Task<IActionResult> UpdateAnuncio(Guid id,RequestAnuncio anuncio)
        {
            try
            {
                var anuncioQuery = await _anuncioRepository.GetAnuncioById(id);
                if (anuncioQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                await _anuncioRepository.UpdateAnuncio(id, anuncio);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { anuncio = anuncio}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        
        
    }
    
}
