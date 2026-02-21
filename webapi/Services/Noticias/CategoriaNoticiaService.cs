using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Model;
using DaccApi.Model.Requests.Noticias;
using DaccApi.Model.Responses.Noticia;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.Noticias
{
    public interface ICategoriaNoticiaService
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(Guid id);
        Task<IActionResult> Create(RequestCreateCategoriaNoticia request);
        Task<IActionResult> Update(Guid id, RequestUpdateCategoriaNoticia request);
        Task<IActionResult> Delete(Guid id);
    }

    public class CategoriaNoticiaService : ICategoriaNoticiaService
    {
        private readonly ICategoriaNoticiaRepository _repository;

        public CategoriaNoticiaService(ICategoriaNoticiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                var response = list.Select(e => new ResponseCategoriaNoticia(e));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { categories = response }));
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
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Categoria não encontrada.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { category = new ResponseCategoriaNoticia(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Create(RequestCreateCategoriaNoticia request)
        {
            try
            {
                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe uma categoria com este nome.");

                var entity = new CategoriaNoticia { Id = Guid.NewGuid(), Nome = request.Name };
                var success = await _repository.CreateAsync(entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao criar categoria.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { category = new ResponseCategoriaNoticia(entity) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> Update(Guid id, RequestUpdateCategoriaNoticia request)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Categoria não encontrada.");

                var existing = await _repository.GetByNomeAsync(request.Name);
                if (existing != null && existing.Id != id) return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outra categoria com este nome.");

                entity.Nome = request.Name;
                var success = await _repository.UpdateAsync(id, entity);
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao atualizar categoria.");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { category = new ResponseCategoriaNoticia(entity) }));
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
                if (!success) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Categoria não encontrada ou falha ao remover.");
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Categoria removida com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
