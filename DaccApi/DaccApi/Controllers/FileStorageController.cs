using DaccApi.Helpers;
using DaccApi.Services.FileStorage;
using Helpers.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _storageService;

        public FileStorageController(IFileStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("uploadImage")]
        [Authorize(Roles = "administrador")]
        [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB
        public async Task<IActionResult> UploadImageFile([FromBody] IFormFile file)
        {
            if (file.Length == 0)
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