using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Professores;
using DaccApi.Helpers;
using DaccApi.Model.Objects;
using DaccApi.Model.Responses;
using DaccApi.Model.Requests;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Professores
{
    public class ProfessoresService : IProfessoresService
    {
        private readonly IProfessoresRepository _professoresRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProfessoresService(IProfessoresRepository professoresRepository, IFileStorageService fileStorageService)
        {
            _professoresRepository = professoresRepository;
            _fileStorageService = fileStorageService;
        }
        public async Task<IActionResult> GetAllProfessores()
        {
            try
            {
                var professores = await _professoresRepository.GetAllAsync();

                if (professores.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Professor>()));
                
                var response = professores.Select(p => new ResponseProfessor(p)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { faculty = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
            
        }

        public async Task<IActionResult> CreateProfessor(RequestProfessor request)
        {
            try
            {

                var professor = Professor.FromRequest(request);
                professor.Id = Guid.NewGuid();

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        professor.ImagemUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        professor.ImagemUrl = request.ImageUrl;
                    }
                }
                else
                {
                    professor.ImagemUrl = string.Empty;
                }

                await _professoresRepository.CreateAsync(professor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseProfessor(professor)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteProfessor(Guid id)
        {

            try
            {
                var professor = await _professoresRepository.GetByIdAsync(id);
            
                if (professor == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                await _professoresRepository.DeleteAsync(id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }


        public async Task<IActionResult> GetProfessorById(Guid id)
        {
            try
            {
                var professor = await _professoresRepository.GetByIdAsync(id);

                
                if (professor == null) 
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Professor>()));

                var response = new ResponseProfessor(professor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { facultyMember = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateProfessor(Guid id, RequestProfessor request)
        {
            try
            {
                var professorQuery = await _professoresRepository.GetByIdAsync(id);
                if (professorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var professor = Professor.FromRequest(request);

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    if (request.ImageUrl.StartsWith("data:image") || request.ImageUrl.Length > 255)
                    {
                        professor.ImagemUrl = await _fileStorageService.SaveBase64ImageAsync(request.ImageUrl);
                    }
                    else
                    {
                        professor.ImagemUrl = request.ImageUrl;
                    }
                }
                
                // Preserve original creation date and set correct Kind
                professor.DataCriacao = DateTime.SpecifyKind(professorQuery.DataCriacao, DateTimeKind.Utc);
                professor.DataAtualizacao = DateTime.UtcNow;

                await _professoresRepository.UpdateAsync(id, professor);

                var response = new ResponseProfessor(professor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { facultyMember = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateProfessorJson(RequestProfessorJson request)
        {
            try
            {
                var professor = new Professor
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Name,
                    Titulo = request.Title,
                    Cargo = request.Position,
                    Especializacao = request.Specialization,
                    ImagemUrl = request.ImageUrl, // URL já fornecida ou null
                    Email = request.Social?.Email,
                    Linkedin = request.Social?.Linkedin,
                    Github = request.Social?.Github,
                    UserId = request.UserId
                };

                await _professoresRepository.CreateAsync(professor);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseProfessor(professor)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateProfessorJson(Guid id, RequestProfessorJson request)
        {
            try
            {
                var professorQuery = await _professoresRepository.GetByIdAsync(id);
                if (professorQuery == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                var professor = new Professor
                {
                    Nome = request.Name,
                    Titulo = request.Title,
                    Cargo = request.Position,
                    Especializacao = request.Specialization,
                    ImagemUrl = request.ImageUrl ?? professorQuery.ImagemUrl, // Mantém a URL existente se não fornecida
                    Email = request.Social?.Email,
                    Linkedin = request.Social?.Linkedin,
                    Github = request.Social?.Github,
                    UserId = request.UserId
                };

                await _professoresRepository.UpdateAsync(id, professor);

                var response = new ResponseProfessor(professor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { facultyMember = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }


        public async Task<IActionResult> SearchProfessores(RequestQueryProfessor query)
        {
            try
            {
                var (professores, totalCount) = await _professoresRepository.SearchProfessores(query);

                if (professores.Count == 0 && totalCount == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Professor>()));

                var response = professores.Select(p => new ResponseProfessor(p)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { faculty = response, totalCount = totalCount }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
