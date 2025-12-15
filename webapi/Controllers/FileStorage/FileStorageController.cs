using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Responses;
using DaccApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _storageService;

        public FileStorageController(IFileStorageService storageService)
        {
            _storageService = storageService;
        }

        [FileUploadResponses]
        [HttpPost("uploadImage")]
        [Authorize(Roles = "administrador")]
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