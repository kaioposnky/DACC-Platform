using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Diretores;
using DaccApi.Helpers;
using DaccApi.Model.Objects;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public class DiretoresService : IDiretoresService
    {
        private readonly IDiretoresRepository _diretoresRepository;
        private readonly IFileStorageService _fileStorageService;

        public DiretoresService(IDiretoresRepository diretoresRepository, IFileStorageService fileStorageService)
        {
            _diretoresRepository = diretoresRepository;
            _fileStorageService = fileStorageService;
        }
        public async Task<IActionResult> GetAllDiretores()
        {
            try
            {
                var diretores = await _diretoresRepository.GetAllAsync();

                if (diretores.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Diretor>()));
                
                var response = diretores.Select(d => new ResponseDiretor(d)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { faculty = response }));
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

                var diretor = Diretor.FromRequest(request);
                diretor.Id = Guid.NewGuid();

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        diretor.ImageUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        diretor.ImageUrl = request.ImageUrl;
                    }
                }
                else
                {
                    diretor.ImageUrl = string.Empty;
                }

                await _diretoresRepository.CreateAsync(diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseDiretor(diretor)));
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
                var diretor = await _diretoresRepository.GetByIdAsync(id);
            
                if (diretor == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                await _diretoresRepository.DeleteAsync(id);

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
                var diretor = await _diretoresRepository.GetByIdAsync(id);

                
                if (diretor == null) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Diretor>()));

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { facultyMember = response }));
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
                var diretorQuery = await _diretoresRepository.GetByIdAsync(id);
                if (diretorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var diretor = Diretor.FromRequest(request);

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        diretor.ImageUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        diretor.ImageUrl = request.ImageUrl;
                    }
                }
                
                // Preserve original creation date and set correct Kind
                diretor.DataCriacao = DateTime.SpecifyKind(diretorQuery.DataCriacao, DateTimeKind.Utc);
                diretor.DataAtualizacao = DateTime.UtcNow;

                await _diretoresRepository.UpdateAsync(id, diretor);

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { facultyMember = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
            }
        }

        public async Task<IActionResult> CreateDiretorJson(RequestDiretorJson request)
        {
            try
            {
                var diretor = new Diretor
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Name,
                    Titulo = request.Title,
                    Cargo = request.Position,
                    Especializacao = request.Specialization,
                    ImageUrl = request.ImageUrl, // URL já fornecida ou null
                    Email = request.Email,
                    Linkedin = request.Linkedin,
                    Github = request.Github,
                    UserId = request.UserId
                };

                await _diretoresRepository.CreateAsync(diretor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseDiretor(diretor)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateDiretorJson(Guid id, RequestDiretorJson request)
        {
            try
            {
                var diretorQuery = await _diretoresRepository.GetByIdAsync(id);
                if (diretorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var diretor = new Diretor
                {
                    Nome = request.Name,
                    Titulo = request.Title,
                    Cargo = request.Position,
                    Especializacao = request.Specialization,
                    ImageUrl = request.ImageUrl ?? diretorQuery.ImageUrl, // Mantém a URL existente se não fornecida
                    Email = request.Email,
                    Linkedin = request.Linkedin,
                    Github = request.Github,
                    UserId = request.UserId
                };

                await _diretoresRepository.UpdateAsync(id, diretor);

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { facultyMember = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }


        public async Task<IActionResult> SearchDiretores(Model.Requests.RequestQueryDiretor query)
        {
            try
            {
                var (diretores, totalCount) = await _diretoresRepository.SearchDiretores(query);

                if (diretores.Count == 0 && totalCount == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Diretor>()));

                var response = diretores.Select(d => new ResponseDiretor(d)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { faculty = response, totalCount = totalCount }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
