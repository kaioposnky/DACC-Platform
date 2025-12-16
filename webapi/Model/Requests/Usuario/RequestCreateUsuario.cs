using System.ComponentModel.DataAnnotations;
using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar um novo usuário.
    /// </summary>
    public class RequestCreateUsuario
    {
        /// <summary>
        /// Obtém ou define o nome do usuário.
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(20, ErrorMessage = "Nome deve ter no máximo 20 caracteres")]
        [MinLength(3, ErrorMessage = "Nome deve ter pelo menos 3 caracteres")]
        public string? Nome { get; set; }
        /// <summary>
        /// Obtém ou define o sobrenome do usuário.
        /// </summary>
        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        [StringLength(20, ErrorMessage = "Sobrenome deve ter no máximo 20 caracteres")]
        [MinLength(3, ErrorMessage = "Sobrenome deve ter pelo menos 3 caracteres")]
        public string? Sobrenome { get; set; }
        /// <summary>
        /// Obtém ou define o e-mail do usuário.
        /// </summary>
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }
        /// <summary>
        /// Obtém ou define o RA (Registro do Aluno) do usuário.
        /// </summary>
        [RaValidation(ErrorMessage = "RA inválido")]
        public string? Ra { get; set; }
        /// <summary>
        /// Obtém ou define o curso do usuário.
        /// </summary>
        public string? Curso { get; set; }
        /// <summary>
        /// Obtém ou define o telefone do usuário.
        /// </summary>
        [Required(ErrorMessage = "Telefone é obrigatório")]
        [PhoneValidation(ErrorMessage = "Telefone inválido")]
        public string? Telefone { get; set; }
        /// <summary>
        /// Obtém ou define a senha do usuário.
        /// </summary>
        public string? Senha { get; set; }
        /// <summary>
        /// Obtém ou define se o usuário deseja se inscrever na newsletter.
        /// </summary>
        public bool? InscritoNoticia { get; set; } = false;
        /// <summary>
        /// Obtém ou define o cargo do usuário.
        /// </summary>
        [CargoValido]
        public string? Cargo {  get; set; }
    }
}
