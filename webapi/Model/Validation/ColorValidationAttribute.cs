using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DaccApi.Model.Validation
{
    public partial class ColorValidationAttribute : ValidationAttribute
    {
        private readonly string[] _forbiddenWords = { "admin", "null", "undefined", "test" };
        private readonly Regex _colorNameRegex = MyRegex();

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            if (value is not string colorName)
            {
                return new ValidationResult("Nome da cor deve ser uma string");
            }

            if (colorName.Trim().Length < 2)
            {
                return new ValidationResult("Nome da cor deve ter pelo menos 2 caracteres");
            }

            if (colorName.Length > 50)
            {
                return new ValidationResult("Nome da cor deve ter no máximo 50 caracteres");
            }

            if (!_colorNameRegex.IsMatch(colorName))
            {
                return new ValidationResult("Nome da cor deve conter apenas letras, espaços e hífens");
            }

            var lowerColorName = colorName.ToLowerInvariant();
            if (_forbiddenWords.Any(word => lowerColorName.Contains(word)))
            {
                return new ValidationResult("Nome da cor contém palavras não permitidas");
            }

            if (colorName != colorName.Trim() || colorName.Contains("  "))
            {
                return new ValidationResult("Nome da cor não deve ter espaços extras no início, fim ou consecutivos");
            }

            return ValidationResult.Success!;
        }

        [GeneratedRegex("^[a-zA-ZÀ-ÿ\\s\\-]+$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}