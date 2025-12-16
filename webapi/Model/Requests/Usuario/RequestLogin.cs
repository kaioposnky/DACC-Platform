using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição de login de usuário.
    /// </summary>
    public class RequestLogin
    {
        /// <summary>
        /// Obtém ou define o e-mail do usuário.
        /// </summary>
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Obtém ou define a senha do usuário.
        /// </summary>
        [Required]
        public string Senha { get; set; }
    }
}