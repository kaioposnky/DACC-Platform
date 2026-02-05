using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Model.Objects;
using DaccApi.Model.Requests.Projetos;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public class DiretoriaService : IDiretoriaService
    {
        private readonly IDiretoriaRepository _diretoriaRepository;

        public DiretoriaService(IDiretoriaRepository diretoriaRepository)
        {
            _diretoriaRepository = diretoriaRepository;
        }

        public async Task<IActionResult> GetAllDiretorias()
        {
            try
            {
                var diretorias = await _diretoriaRepository.GetAllAsync();
                var response = diretorias.Select(d => new ResponseDiretoria(d));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { directorates = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetDiretoriaById(Guid id)
        {
            try
            {
                var diretoria = await _diretoriaRepository.GetByIdAsync(id);
                if (diretoria == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                
                var response = new ResponseDiretoria(diretoria);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { directorate = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> SearchDiretorias(RequestQueryDiretoria query)
        {
            try
            {
                var (diretorias, totalCount) = await _diretoriaRepository.SearchAsync(query);
                var response = diretorias.Select(d => new ResponseDiretoria(d));
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new 
                { 
                    directorates = response,
                    totalCount,
                    page = query.Page ?? 1,
                    limit = query.Limit ?? 16
                }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateDiretoria(RequestCreateDiretoria request)
        {
            try
            {
                var diretoria = new Diretoria
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Name,
                    Descricao = request.Description
                };

                var created = await _diretoriaRepository.CreateAsync(diretoria);
                if (!created) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao criar diretoria");

                var response = new ResponseDiretoria(diretoria);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.CREATED, new { directorate = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateDiretoria(Guid id, RequestUpdateDiretoria request)
        {
            try
            {
                var existing = await _diretoriaRepository.GetByIdAsync(id);
                if (existing == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);

                existing.Nome = request.Name ?? existing.Nome;
                existing.Descricao = request.Description ?? existing.Descricao;

                var updated = await _diretoriaRepository.UpdateAsync(id, existing);
                if (!updated) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao atualizar diretoria");

                var response = new ResponseDiretoria(existing);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { directorate = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteDiretoria(Guid id)
        {
            try
            {
                var existing = await _diretoriaRepository.GetByIdAsync(id);
                if (existing == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);

                var deleted = await _diretoriaRepository.DeleteAsync(id);
                if (!deleted) return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Falha ao deletar diretoria");

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
