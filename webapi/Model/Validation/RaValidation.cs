using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DaccApi.Model.Validation
{
    /// <summary>
    /// Valida um RA (Registro de Aluno).
    /// </summary>
    public partial class RaValidation : ValidationAttribute
    {
        /// <summary>
        /// Valida o valor do RA.
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var ra = value as string;
            if (value == null || string.IsNullOrEmpty(ra))
            {
                return ValidationResult.Success;
            }

            if (ra.Length == 9 && RaRegex().IsMatch(ra))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Formato de ra inválido.");
        }
        
        
        [GeneratedRegex(@"^[1-9]\d{8}$")]
        private static partial Regex RaRegex();
    }
}