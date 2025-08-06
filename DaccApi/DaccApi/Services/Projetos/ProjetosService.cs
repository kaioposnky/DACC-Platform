using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Projetos;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;

namespace DaccApi.Services.Projetos
{
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetosRepository;

        public ProjetosService(IProjetosRepository projetosRepository)
        {
            _projetosRepository = projetosRepository;
        }
        public async Task<IActionResult> GetAllProjetos()
        {
            try
            {
                var projetos = await _projetosRepository.GetAllProjetos();
                
                if (projetos.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);


                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { Projetos = projetos }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
            
        }

        public async Task<IActionResult> GetProjetoById(Guid id)
        {
            try
            {
                var projeto = await _projetosRepository.GetProjetoById(id);
                if (projeto == null)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projetos = projeto}));
                
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }


        public async Task<IActionResult> CreateProjeto(RequestProjeto request)
        {
            try
            {
                
                if (String.IsNullOrWhiteSpace(request.Titulo) ||
                    String.IsNullOrWhiteSpace(request.Descricao) ||
                    request.ImageFile == null ||
                    String.IsNullOrWhiteSpace(request.Status)||
                    String.IsNullOrWhiteSpace(request.Diretoria)||
                    request.Tags == null)
                    
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }


                await _projetosRepository.CreateProjeto(projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> DeleteProjeto(Guid id)
        {
            try
            {
                var projeto = await _projetosRepository.GetProjetoById(id);
            
                if (projeto == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                _projetosRepository.DeleteProjeto(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> UpdateProjeto(Guid id, RequestProjeto projeto)
        {
            try
            {
                var projetoQuery = await _projetosRepository.GetProjetoById(id);
                if (projetoQuery == null ||
                    String.IsNullOrWhiteSpace(projeto.Titulo) ||
                    String.IsNullOrWhiteSpace(projeto.Descricao) ||
                    projeto.ImageFile == null ||
                    String.IsNullOrWhiteSpace(projeto.Status)||
                    String.IsNullOrWhiteSpace(projeto.Diretoria)||
                    projeto.Tags == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                _projetosRepository.UpdateProjeto(id, projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projetos = projeto}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

    }
}
