using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DaccApi.Model.Validation
{
    /// <summary>
    /// Valida um número de telefone.
    /// </summary>
    public partial class PhoneValidation : ValidationAttribute
    {
        /// <summary>
        /// Valida o valor do número de telefone.
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var phone = value as string;

            // Validação de telefone, é válido se o número tem 10 ou 11 dígitos ou mais e começa com um número.
            if (string.IsNullOrWhiteSpace(phone) || PhoneRegex().IsMatch(phone))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Número de telefone inválido.");
        }

        // Formato válido: 11950166444
        [GeneratedRegex(@"^([1-9]{1})(\d{9,10})$")]
        private static partial Regex PhoneRegex();
    }
}