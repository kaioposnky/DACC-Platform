using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Projetos;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Model.Requests;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;

namespace DaccApi.Services.Projetos
{
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetosRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProjetosService(IProjetosRepository projetosRepository, IFileStorageService fileStorageService)
        {
            _projetosRepository = projetosRepository;
            _fileStorageService = fileStorageService;
        }
        public async Task<IActionResult> GetAllProjetos()
        {
            try
            {
                var projetos = await _projetosRepository.GetAllAsync();
                
                if (projetos.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                var response = projetos.Select(projeto => new ResponseProjeto(projeto));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projetos = response }));
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
                var projeto = await _projetosRepository.GetByIdAsync(id);
                if (projeto == null)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                var response = new ResponseProjeto(projeto);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projeto = response}));
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
                    String.IsNullOrWhiteSpace(request.Status)||
                    String.IsNullOrWhiteSpace(request.Diretoria)||
                    request.Tags == null)
                    
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var projeto = new Projeto()
                {
                    Id = Guid.NewGuid(),
                    Titulo = request.Titulo,
                    Descricao = request.Descricao,
                    Status = request.Status,
                    Diretoria = request.Diretoria,
                    Tags = request.Tags,
                    TextoConclusao = request.TextoConclusao ?? string.Empty,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };
                
                await _projetosRepository.CreateAsync(projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        
        public async Task<IActionResult> AddProjetoImage(Guid id, ImageRequest request)
        {
            try
            {
                var imageUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);

                var projeto = await _projetosRepository.GetByIdAsync(id);

                if (projeto == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Projeto não encontrada!");
                }
            
                projeto.ImagemUrl = imageUrl;
                projeto.ImagemAlt = request.ImageAlt;
            
                await _projetosRepository.UpdateAsync(id, projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao adicionar anuncio na projeto." + ex.Message);
            }
        }
        

        public async Task<IActionResult> DeleteProjeto(Guid id)
        {
            try
            {
                var projeto = await _projetosRepository.GetByIdAsync(id);
            
                if (projeto == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                await _projetosRepository.DeleteAsync(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> UpdateProjeto(Guid id, RequestProjeto request)
        {
            try
            {
                var projetoQuery = await _projetosRepository.GetByIdAsync(id);
                if (projetoQuery == null ||
                    String.IsNullOrWhiteSpace(request.Titulo) ||
                    String.IsNullOrWhiteSpace(request.Descricao) ||
                    String.IsNullOrWhiteSpace(request.Status)||
                    String.IsNullOrWhiteSpace(request.Diretoria)||
                    request.Tags == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                
                projetoQuery.Titulo = request.Titulo;
                projetoQuery.Descricao = request.Descricao;
                projetoQuery.Status = request.Status;
                projetoQuery.Diretoria = request.Diretoria;
                projetoQuery.Tags = request.Tags;
                projetoQuery.TextoConclusao = request.TextoConclusao ?? projetoQuery.TextoConclusao;
                projetoQuery.DataAtualizacao = DateTime.UtcNow;
                
                await _projetosRepository.UpdateAsync(id, projetoQuery);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

    }
}
