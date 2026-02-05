using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model.Objects;
using DaccApi.Model.Requests.Curso;
using DaccApi.Model.Responses.Curso;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.User
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<IActionResult> GetAllCursos()
        {
            try
            {
                var cursos = await _cursoRepository.GetAllAsync();
                var response = cursos.Select(c => new ResponseCurso(c));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { cursos = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetCursoById(Guid id)
        {
            try
            {
                var curso = await _cursoRepository.GetByIdAsync(id);
                if (curso == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Curso não encontrado.");
                }
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { curso = new ResponseCurso(curso) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateCurso(RequestCreateCurso request)
        {
            try
            {
                var existing = await _cursoRepository.GetByNomeAsync(request.Nome);
                if (existing != null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe um curso com este nome.");
                }

                var curso = new Curso { Nome = request.Nome };
                var id = await _cursoRepository.CreateAsync(curso);
                curso.Id = id;

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { curso = new ResponseCurso(curso) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateCurso(Guid id, RequestUpdateCurso request)
        {
            try
            {
                var curso = await _cursoRepository.GetByIdAsync(id);
                if (curso == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Curso não encontrado.");
                }

                var existing = await _cursoRepository.GetByNomeAsync(request.Nome);
                if (existing != null && existing.Id != id)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outro curso com este nome.");
                }

                curso.Nome = request.Nome;
                await _cursoRepository.UpdateAsync(id, curso);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { curso = new ResponseCurso(curso) }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteCurso(Guid id)
        {
            try
            {
                var curso = await _cursoRepository.GetByIdAsync(id);
                if (curso == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Curso não encontrado.");
                }

                await _cursoRepository.DeleteAsync(id);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Curso excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
