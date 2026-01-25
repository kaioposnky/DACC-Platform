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
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                
                var response = diretores.Select(d => new ResponseDiretor(d)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretores = response}));
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

                if (request.ImageFile != null)
                {
                    diretor.ImagemUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);
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
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { diretor = response}));
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

                if (request.ImageFile != null)
                {
                    diretor.ImagemUrl = await _fileStorageService.SaveImageFileAsync(request.ImageFile);
                }

                await _diretoresRepository.UpdateAsync(id, diretor);

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { diretor = response}));
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
                    Nome = request.Nome,
                    Titulo = request.Titulo,
                    Cargo = request.Cargo,
                    Especializacao = request.Especializacao,
                    ImagemUrl = request.ImagemUrl, // URL já fornecida ou null
                    Email = request.Email,
                    Linkedin = request.Linkedin,
                    Github = request.Github,
                    UsuarioId = request.UsuarioId
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
                    Nome = request.Nome,
                    Titulo = request.Titulo,
                    Cargo = request.Cargo,
                    Especializacao = request.Especializacao,
                    ImagemUrl = request.ImagemUrl ?? diretorQuery.ImagemUrl, // Mantém a URL existente se não fornecida
                    Email = request.Email,
                    Linkedin = request.Linkedin,
                    Github = request.Github,
                    UsuarioId = request.UsuarioId
                };

                await _diretoresRepository.UpdateAsync(id, diretor);

                var response = new ResponseDiretor(diretor);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { diretor = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
