using System.Collections;
using System.ComponentModel.DataAnnotations;
using NHibernate.Util;

namespace DaccApi.Model.Validation
{
    public class CustomAllowedValuesAttribute : ValidationAttribute
    {
        private readonly HashSet<object> _validValues;
        public CustomAllowedValuesAttribute(params object[] validValues)
        {
            _validValues = new HashSet<object>(validValues);
        }
        
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