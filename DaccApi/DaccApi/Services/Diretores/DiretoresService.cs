using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Repositories.Diretores;
using DaccApi.Helpers;
using DaccApi.Services.Diretores;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public class DiretoresService : IDiretoresService
    {
        private readonly IDiretoresRepository _diretoresRepository;

        public DiretoresService(IDiretoresRepository diretoresRepository)
        {
            _diretoresRepository = diretoresRepository;
        }
        public async Task<IActionResult> GetAllDiretores()
        {
            try
            {
                var diretores = await _diretoresRepository.GetAllDiretores();

                if (diretores.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretores = diretores}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
            
        }

        public async Task<IActionResult> CreateDiretor(RequestDiretor request)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(request.Nome) ||
                    String.IsNullOrWhiteSpace(request.Descricao) ||
                    String.IsNullOrWhiteSpace(request.GithubLink)||
                    String.IsNullOrWhiteSpace(request.LinkedinLink)||
                    String.IsNullOrWhiteSpace(request.Email)||
                    request.DiretoriaId == null||
                    request.UsuarioId == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var diretor = new Diretor()
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    GithubLink = request.GithubLink,
                    LinkedinLink = request.LinkedinLink,
                    Email = request.Email,
                    DiretoriaId = request.DiretoriaId,
                    UsuarioId = request.UsuarioId,
                };
                
                await _diretoresRepository.CreateDiretor(diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> DeleteDiretor(Guid id)
        {

            try
            {
                var diretor = await _diretoresRepository.GetDiretorById(id);
            
                if (diretor == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                _diretoresRepository.DeleteDiretor(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }


        public async Task<IActionResult> GetDiretorById(Guid id)
        {
            try
            {
                var diretor = await _diretoresRepository.GetDiretorById(id);

                
                if (diretor == null) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretor = diretor}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> UpdateDiretor(Guid id, RequestDiretor request)
        {
            try
            {
                var diretorQuery = await _diretoresRepository.GetDiretorById(id);
                if (diretorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                
                var diretor = new Diretor()
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    GithubLink = request.GithubLink,
                    LinkedinLink = request.LinkedinLink,
                    Email = request.Email,
                    DiretoriaId = request.DiretoriaId,
                    UsuarioId = request.UsuarioId,
                };
                
                await _diretoresRepository.UpdateDiretor(id, diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { diretor = request}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

       
    }
}
