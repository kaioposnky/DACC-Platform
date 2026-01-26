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
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Arquivo inválido ou vazio.");
            }

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

            try
            {
                await using var stream = file.OpenReadStream();
                using var image = await Image.LoadAsync(stream);
                await image.SaveAsWebpAsync(filePath);
            }
            catch (UnknownImageFormatException)
            {
                 throw new ArgumentException("Formato de imagem inválido ou não suportado.");
            }
            catch (Exception ex)
            {
                // Logar o erro real aqui seria ideal
                Console.WriteLine($"Erro ao processar imagem: {ex}");
                throw new ArgumentException("Falha ao processar o arquivo de imagem.", ex);
            }

            // Construção segura da URL
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                // Fallback para contextos sem HTTP (ex: testes, background jobs)
                return $"/{subfolder}/{uniqueFileName}";
            }

            var url = $"{request.Scheme}://{request.Host}/{subfolder}/{uniqueFileName}";
            return url;
        }
    }
}