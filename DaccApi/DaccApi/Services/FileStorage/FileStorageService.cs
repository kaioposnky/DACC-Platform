using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace DaccApi.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public FileStorageService(IWebHostEnvironment environment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveImageFileAsync(IFormFile file)
        {
            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException("Arquivo excede o tamanho máximo de 5MB.");
            }

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(_environment.WebRootPath))
                {
                    Directory.CreateDirectory(_environment.WebRootPath);
                }
            }
    
            var subfolder = _configuration["UploadFilesSubfolder"] ?? "uploads";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, subfolder);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            
            const string extension = ".webp";
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = file.OpenReadStream())
            {
                try
                {
                    var image = await Image.LoadAsync(stream);
                    await image.SaveAsWebpAsync(filePath);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Formato de imagem inválido!",ex.Message);
                }
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            var url = $"{request.Scheme}://{request.Host}/{subfolder}/{uniqueFileName}";

            return url;
        }
    }
}