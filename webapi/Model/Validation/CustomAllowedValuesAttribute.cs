using System.Collections;
using System.ComponentModel.DataAnnotations;
using NHibernate.Util;

namespace DaccApi.Model.Validation
{
    /// <summary>
    /// Valida se um valor está contido em uma lista de valores permitidos.
    /// </summary>
    public class CustomAllowedValuesAttribute : ValidationAttribute
    {
        private readonly HashSet<object> _validValues;
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CustomAllowedValuesAttribute"/>.
        /// </summary>
        /// <param name="validValues">Os valores permitidos.</param>
        public CustomAllowedValuesAttribute(params object[] validValues)
        {
            _validValues = new HashSet<object>(validValues);
        }
        
        /// <summary>
        /// Valida o valor fornecido.
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }
            
            if (!_validValues.Contains(value))
            {
                return new ValidationResult($"O valor '{value}' não é válido. Valores permitidos: {string.Join(", ", _validValues)}");
            }
            
            return ValidationResult.Success!;
        }
    }
}