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
        public IActionResult GetAllProjetos()
        {
            try
            {
                var projetos = _projetosRepository.GetAllProjetos().Result;
                
                if (projetos.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);


                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { Projetos = projetos }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
            
        }

        public IActionResult GetProjetoById(int id)
        {
            try
            {
                var projeto = _projetosRepository.GetProjetoById(id).Result;
                if (projeto == null)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projetos = projeto}));
                
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }


        public IActionResult CreateProjeto(RequestProjeto projeto)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(projeto.Titulo) ||
                    String.IsNullOrWhiteSpace(projeto.Descricao) ||
                    String.IsNullOrWhiteSpace(projeto.Imagem_url) ||
                    String.IsNullOrWhiteSpace(projeto.Status)||
                    String.IsNullOrWhiteSpace(projeto.Diretoria) ||
                    projeto.Tags == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
            
                _projetosRepository.CreateProjeto(projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

        public IActionResult DeleteProjeto(int id)
        {
            try
            {
                var projeto = _projetosRepository.GetProjetoById(id).Result;
            
                if (projeto == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                _projetosRepository.DeleteProjeto(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

        public IActionResult UpdateProjeto(int id, RequestProjeto projeto)
        {
            try
            {
                var projetoQuery = _projetosRepository.GetProjetoById(id).Result;
                if (projetoQuery == null ||
                    String.IsNullOrWhiteSpace(projeto.Titulo) ||
                    String.IsNullOrWhiteSpace(projeto.Descricao) ||
                    String.IsNullOrWhiteSpace(projeto.Imagem_url) ||
                    String.IsNullOrWhiteSpace(projeto.Status)||
                    String.IsNullOrWhiteSpace(projeto.Diretoria) ||
                    projeto.Tags == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                _projetosRepository.UpdateProjeto(id, projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projetos = projeto}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }

    }
}
