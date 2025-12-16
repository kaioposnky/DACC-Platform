using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DaccApi.Model.Validation
{
    /// <summary>
    /// Valida arquivos de imagem com base no tamanho, contagem e extensão.
    /// </summary>
    public class ImageValidationAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize; // padrão 5MB
        private readonly int _maxFileCount;
        private readonly string[] _allowedExtensions;
        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ImageValidationAttribute"/>.
        /// </summary>
        /// <param name="maxFileSize">O tamanho máximo do arquivo em bytes.</param>
        /// <param name="maxFileCount">O número máximo de arquivos permitidos.</param>
        public ImageValidationAttribute(long maxFileSize = 5 * 1024 * 1024, int maxFileCount = 10)
        {
            _maxFileSize = maxFileSize;
            _maxFileCount = maxFileCount;
            _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        }

        /// <summary>
        /// Valida os arquivos de imagem fornecidos.
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            if (value is not IFormFile[] files)
            {
                return new ValidationResult("Valor deve ser um array de arquivos");
            }

            if (files.Length > _maxFileCount)
            {
                return new ValidationResult($"Máximo de {_maxFileCount} imagens permitidas");
            }

            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                
                if (file.Length == 0)
                {
                    return new ValidationResult($"Arquivo {i + 1} está vazio");
                }

                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Arquivo '{file.FileName}' excede o tamanho máximo de {_maxFileSize / (1024 * 1024)}MB");
                }

                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Arquivo '{file.FileName}' possui formato inválido. Formatos aceitos: {string.Join(", ", _allowedExtensions)}");
                }

                if (!IsValidImageContentType(file.ContentType))
                {
                    return new ValidationResult($"Arquivo '{file.FileName}' não é uma imagem válida");
                }
            }

            return ValidationResult.Success!;
        }

        private static bool IsValidImageContentType(string contentType)
        {
            var validContentTypes = new[]
            {
                "image/jpeg",
                "image/jpg", 
                "image/png",
                "image/webp"
            };

            return validContentTypes.Contains(contentType?.ToLowerInvariant());
        }
    }
}