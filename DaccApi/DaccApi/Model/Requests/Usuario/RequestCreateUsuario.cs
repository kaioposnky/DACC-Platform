using System.ComponentModel.DataAnnotations;
using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestCreateUsuario
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(20, ErrorMessage = "Nome deve ter no máximo 20 caracteres")]
        [MinLength(3, ErrorMessage = "Nome deve ter pelo menos 3 caracteres")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        [StringLength(20, ErrorMessage = "Sobrenome deve ter no máximo 20 caracteres")]
        [MinLength(3, ErrorMessage = "Sobrenome deve ter pelo menos 3 caracteres")]
        public string? Sobrenome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Ra { get; set; }
        public string? Curso { get; set; }
        [Phone]
        public string? Telefone { get; set; }
        public string? Senha { get; set; }
        public bool? InscritoNoticia { get; set; } = false;
        [CargoValido]
        public string? Cargo {  get; set; }
    }
}
