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
        public IActionResult GetAllDiretores()
        {
            try
            {
                var diretores = _diretoresRepository.GetAllDiretores().Result;

                if (diretores.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretores = diretores}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
            
        }

        public IActionResult CreateDiretor(RequestDiretor diretor)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(diretor.Nome) ||
                    String.IsNullOrWhiteSpace(diretor.Descricao) ||
                    String.IsNullOrWhiteSpace(diretor.Github_link)||
                    String.IsNullOrWhiteSpace(diretor.Linkedin_link)||
                    String.IsNullOrWhiteSpace(diretor.Email)||
                    diretor.Diretoria_id == null||
                    diretor.Usuario_id == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
            
                _diretoresRepository.CreateDiretor(diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

        public IActionResult DeleteDiretor(int id)
        {

            try
            {
                var diretor = _diretoresRepository.GetDiretorById(id).Result;
            
                if (diretor == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                _diretoresRepository.DeleteDiretor(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }


        public IActionResult GetDiretorById(int id)
        {
            try
            {
                var diretor = _diretoresRepository.GetDiretorById(id).Result;

                if (diretor == null) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretor = diretor}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

        public IActionResult UpdateDiretor(int id,RequestDiretor diretor)
        {
            try
            {
                var diretorQuery = _diretoresRepository.GetDiretorById(id).Result;
                if (diretorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                _diretoresRepository.UpdateDiretor(id, diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { diretor = diretor}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

       
    }
}
