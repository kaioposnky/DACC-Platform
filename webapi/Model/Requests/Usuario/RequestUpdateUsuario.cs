using System.ComponentModel.DataAnnotations;
using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para atualizar um usuário.
    /// </summary>
    public class RequestUpdateUsuario
    {
        /// <summary>
        /// Obtém ou define o nome do usuário.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Obtém ou define o sobrenome do usuário.
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Obtém ou define o e-mail do usuário.
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }
        /// <summary>
        /// Obtém ou define o curso do usuário.
        /// </summary>
        public string? Course { get; set; }

        /// <summary>
        /// Obtém ou define o RA do usuário.
        /// </summary>
        public string? Ra { get; set; }
        /// <summary>
        /// Obtém ou define o telefone do usuário.
        /// </summary>
        [PhoneValidation(ErrorMessage = "Telefone inválido")]
        public string? Phone { get; set; }
        /// <summary>
        /// Obtém ou define o arquivo de imagem de perfil do usuário.
        /// </summary>
        public IFormFile? ImageFile { get; set; }
        /// <summary>
        /// Obtém ou define se o usuário está inscrito na newsletter.
        /// </summary>
        public bool? IsSubscribedToNews { get; set; }
    }
}
