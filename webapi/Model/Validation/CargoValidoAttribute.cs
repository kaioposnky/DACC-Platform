using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Validation
{
    /// <summary>
    /// Valida se o cargo fornecido está entre os valores permitidos.
    /// </summary>
    public class CargoValidoAttribute : ValidationAttribute

    {
        private readonly string[] _validRoles = { "aluno", "diretor", "administrador" };

        /// <summary>
        /// Valida o valor do cargo.
        /// </summary>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("O cargo não pode ser nulo");;
            }

            if (!_validRoles.Contains(value))
            {
                return new ValidationResult("O cargo deve ser um dos seguintes: " 
                                            + string.Join(", ", _validRoles));
            }
            
            return ValidationResult.Success!;
        }
    }
}