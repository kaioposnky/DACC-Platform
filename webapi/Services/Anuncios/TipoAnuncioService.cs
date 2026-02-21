using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Model;
using DaccApi.Model.Requests.Anuncio;
using DaccApi.Model.Responses.Anuncio;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.Anuncios
{
    public class TipoAnuncioService : ITipoAnuncioService
    {
        private readonly ITipoAnuncioRepository _tipoAnuncioRepository;

        public TipoAnuncioService(ITipoAnuncioRepository tipoAnuncioRepository)
        {
            _tipoAnuncioRepository = tipoAnuncioRepository;
        }

        public async Task<IActionResult> GetAllTiposAnuncio()
        {
            try
            {
                var tipos = await _tipoAnuncioRepository.GetAllAsync();
                var response = tipos.Select(t => new ResponseTipoAnuncio(t));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { types = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetTipoAnuncioById(Guid id)
        {
            try
            {
                var tipo = await _tipoAnuncioRepository.GetByIdAsync(id);
                if (tipo == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de anúncio não encontrado.");
                }
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { type = new ResponseTipoAnuncio(tipo) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateTipoAnuncio(RequestCreateTipoAnuncio request)
        {
            try
            {
                var existing = await _tipoAnuncioRepository.GetByNomeAsync(request.Name);
                if (existing != null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe um tipo de anúncio com este nome.");
                }

                var tipo = new TipoAnuncio { Id = Guid.NewGuid(), Nome = request.Name };
                var success = await _tipoAnuncioRepository.CreateAsync(tipo);
                
                if (!success)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Não foi possível criar o tipo de anúncio.");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { type = new ResponseTipoAnuncio(tipo) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateTipoAnuncio(Guid id, RequestUpdateTipoAnuncio request)
        {
            try
            {
                var tipo = await _tipoAnuncioRepository.GetByIdAsync(id);
                if (tipo == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de anúncio não encontrado.");
                }

                var existing = await _tipoAnuncioRepository.GetByNomeAsync(request.Name);
                if (existing != null && existing.Id != id)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outro tipo de anúncio com este nome.");
                }

                tipo.Nome = request.Name;
                await _tipoAnuncioRepository.UpdateAsync(id, tipo);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { type = new ResponseTipoAnuncio(tipo) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteTipoAnuncio(Guid id)
        {
            try
            {
                var tipo = await _tipoAnuncioRepository.GetByIdAsync(id);
                if (tipo == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de anúncio não encontrado.");
                }

                await _tipoAnuncioRepository.DeleteAsync(id);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Tipo de anúncio excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
