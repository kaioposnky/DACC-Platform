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

            var (filePath, uniqueFileName) = GetUploadPath();

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
                Console.WriteLine($"Erro ao processar imagem: {ex}");
                throw new ArgumentException("Falha ao processar o arquivo de imagem.", ex);
            }

            return BuildUrl(uniqueFileName);
        }

        public async Task<string> SaveBase64ImageAsync(string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
            {
                throw new ArgumentException("Dados base64 inválidos ou vazios.");
            }

            // Remover prefixo data:image/...;base64, se existir
            var base64Content = base64Data.Contains(",") 
                ? base64Data.Split(',')[1] 
                : base64Data;

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64Content);
            }
            catch (FormatException)
            {
                throw new ArgumentException("String Base64 inválida.");
            }

            if (imageBytes.Length > MaxFileSize)
            {
                throw new ArgumentException("Imagem excede o tamanho máximo de 5MB.");
            }

            var (filePath, uniqueFileName) = GetUploadPath();

            try
            {
                using var image = Image.Load(imageBytes);
                await image.SaveAsWebpAsync(filePath);
            }
            catch (UnknownImageFormatException)
            {
                throw new ArgumentException("Formato de imagem Base64 inválido ou não suportado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar imagem Base64: {ex}");
                throw new ArgumentException("Falha ao processar a imagem Base64.", ex);
            }

            return BuildUrl(uniqueFileName);
        }

        private (string filePath, string uniqueFileName) GetUploadPath()
        {
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

            return (filePath, uniqueFileName);
        }

        private string BuildUrl(string uniqueFileName)
        {
            var subfolder = _configuration["UploadFilesSubfolder"] ?? "uploads";
            var request = _httpContextAccessor.HttpContext?.Request;
            
            if (request == null)
            {
                return $"/{subfolder}/{uniqueFileName}";
            }

            return $"{request.Scheme}://{request.Host}/{subfolder}/{uniqueFileName}";
        }
    }
}