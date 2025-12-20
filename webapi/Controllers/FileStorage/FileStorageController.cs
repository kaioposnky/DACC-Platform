using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers
{
    /// <summary>
    /// Controlador para gerenciar o upload de arquivos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _storageService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FileStorageController"/>.
        /// </summary>
        public FileStorageController(IFileStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Realiza o upload de um arquivo de imagem.
        /// </summary>
        [FileUploadResponses]
        [HttpPost("uploadImage")]
        [HasPermission(AppPermissions.FileStorage.UploadImage)]
        [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Nenhum arquivo foi enviado.");
            }

            try
            {
                var fileUrl = await _storageService.SaveImageFileAsync(file);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { url = fileUrl }));
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
            }
        }
    }
}