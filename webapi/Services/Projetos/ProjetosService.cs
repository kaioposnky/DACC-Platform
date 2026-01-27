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
    /// <summary>
    /// Serviço responsável pelo gerenciamento de projetos.
    /// </summary>
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetosRepository;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Construtor do serviço de projetos.
        /// </summary>
        /// <param name="projetosRepository">Repositório de projetos.</param>
        /// <param name="fileStorageService">Serviço de armazenamento de arquivos.</param>
        public ProjetosService(IProjetosRepository projetosRepository, IFileStorageService fileStorageService)
        {
            _projetosRepository = projetosRepository;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Obtém todos os projetos cadastrados.
        /// </summary>
        /// <returns>Lista de projetos ou NoContent se não houver.</returns>
        public async Task<IActionResult> GetAllProjetos()
        {
            try
            {
                var projetos = await _projetosRepository.GetAllAsync();
                
                if (projetos.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                var response = projetos.Select(projeto => new ResponseProjeto(projeto));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { projects = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
            
        }

        /// <summary>
        /// Obtém um projeto pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <returns>Projeto encontrado ou NoContent.</returns>
        public async Task<IActionResult> GetProjetoById(Guid id)
        {
            try
            {
                var projeto = await _projetosRepository.GetByIdAsync(id);
                if (projeto == null)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                var response = new ResponseProjeto(projeto);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { project = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }


        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        /// <param name="request">Dados do projeto.</param>
        /// <returns>Resposta de criação.</returns>
        public async Task<IActionResult> CreateProjeto(RequestProjeto request)
        {
            try
            {
                
                if (String.IsNullOrWhiteSpace(request.Title) ||
                    String.IsNullOrWhiteSpace(request.Description) ||
                    String.IsNullOrWhiteSpace(request.Status)||
                    String.IsNullOrWhiteSpace(request.Department)||
                    request.Tags == null)
                    
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var projeto = new Projeto()
                {
                    Id = Guid.NewGuid(),
                    Titulo = request.Title,
                    Descricao = request.Description,
                    Status = request.Status,
                    Diretoria = request.Department,
                    Tags = request.Tags,
                    TextoConclusao = request.CompletionText ?? string.Empty,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };
                
                await _projetosRepository.CreateAsync(projeto);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseProjeto(projeto)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }
        
        /// <summary>
        /// Adiciona uma imagem a um projeto.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <param name="request">Arquivo de imagem.</param>
        /// <returns>Status da operação.</returns>
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
        

        /// <summary>
        /// Remove um projeto pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <returns>Status da operação.</returns>
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

        /// <summary>
        /// Atualiza um projeto existente.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <param name="request">Novos dados do projeto.</param>
        /// <returns>Status da operação.</returns>
        public async Task<IActionResult> UpdateProjeto(Guid id, RequestProjeto request)
        {
            try
            {
                var projetoQuery = await _projetosRepository.GetByIdAsync(id);
                if (projetoQuery == null ||
                    String.IsNullOrWhiteSpace(request.Title) ||
                    String.IsNullOrWhiteSpace(request.Description) ||
                    String.IsNullOrWhiteSpace(request.Status)||
                    String.IsNullOrWhiteSpace(request.Department)||
                    request.Tags == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                
                projetoQuery.Titulo = request.Title;
                projetoQuery.Descricao = request.Description;
                projetoQuery.Status = request.Status;
                projetoQuery.Diretoria = request.Department;
                projetoQuery.Tags = request.Tags;
                projetoQuery.TextoConclusao = request.CompletionText ?? projetoQuery.TextoConclusao;
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
