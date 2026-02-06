using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Model;
using DaccApi.Model.Requests.Projetos;
using DaccApi.Model.Responses.Projeto;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.Projetos
{
    public interface ITipoProgressoService
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(Guid id);
        Task<IActionResult> Create(RequestCreateTipoProgresso request);
        Task<IActionResult> Update(Guid id, RequestUpdateTipoProgresso request);
        Task<IActionResult> Delete(Guid id);
    }

    public class TipoProgressoService : ITipoProgressoService
    {
        private readonly ITipoProgressoRepository _repository;

        public TipoProgressoService(ITipoProgressoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                var response = list.Select(e => new ResponseTipoProgresso(e));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { progressTypes = response }));
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
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de progresso não encontrado.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { progressType = new ResponseTipoProgresso(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Create(RequestCreateTipoProgresso request)
        {
            try
            {
                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe um tipo de progresso com este nome.");

                var entity = new TipoProgresso { Id = Guid.NewGuid(), Nome = request.Name };
                var success = await _repository.CreateAsync(entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao criar tipo de progresso.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { progressType = new ResponseTipoProgresso(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Update(Guid id, RequestUpdateTipoProgresso request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de progresso não encontrado.");

                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null && existing.Id != id) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outro tipo de progresso com este nome.");

                entity.Nome = request.Name;
                var success = await _repository.UpdateAsync(id, entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao atualizar tipo de progresso.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { progressType = new ResponseTipoProgresso(entity) }));
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
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Tipo de progresso não encontrado ou falha ao remover.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Tipo de progresso removido com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
