using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.Eventos;
using DaccApi.Model;
using DaccApi.Model.Requests.Evento;
using DaccApi.Model.Responses.Evento;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.Eventos
{
    public interface ITipoEventoService
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(Guid id);
        Task<IActionResult> Create(RequestCreateTipoEvento request);
        Task<IActionResult> Update(Guid id, RequestUpdateTipoEvento request);
        Task<IActionResult> Delete(Guid id);
    }

    public class TipoEventoService : ITipoEventoService
    {
        private readonly ITipoEventoRepository _repository;

        public TipoEventoService(ITipoEventoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                var response = list.Select(e => new ResponseTipoEvento(e));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { types = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de evento não encontrado.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { type = new ResponseTipoEvento(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Create(RequestCreateTipoEvento request)
        {
            try
            {
                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe um tipo de evento com este nome.");

                var entity = new TipoEvento { Id = Guid.NewGuid(), Nome = request.Name };
                var success = await _repository.CreateAsync(entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao criar tipo de evento.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { type = new ResponseTipoEvento(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Update(Guid id, RequestUpdateTipoEvento request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de evento não encontrado.");

                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null && existing.Id != id) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outro tipo de evento com este nome.");

                entity.Nome = request.Name;
                var success = await _repository.UpdateAsync(id, entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao atualizar tipo de evento.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { type = new ResponseTipoEvento(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _repository.DeleteAsync(id);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de evento não encontrado ou falha ao remover.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Tipo de evento removido com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
