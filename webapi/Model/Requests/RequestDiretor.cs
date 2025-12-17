using DaccApi.Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um diretor.
    /// </summary>
    public class RequestDiretor
    {
        /// <summary>
        /// Nome do diretor.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// Título acadêmico (Dr., Ms., etc).
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; }

        /// <summary>
        /// Cargo na instituição.
        /// </summary>
        [Required(ErrorMessage = "O cargo é obrigatório")]
        public string Cargo { get; set; }

        /// <summary>
        /// Especialização ou área de atuação.
        /// </summary>
        [Required(ErrorMessage = "A especialização é obrigatória")]
        public string Especializacao { get; set; }

        /// <summary>
        /// Arquivo de imagem de perfil.
        /// </summary>
        [ImageValidation]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Email de contato.
        /// </summary>
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        /// <summary>
        /// Link do LinkedIn.
        /// </summary>
        public string? Linkedin { get; set; }

        /// <summary>
        /// Link do GitHub.
        /// </summary>
        public string? Github { get; set; }

        /// <summary>
        /// ID do usuário vinculado (opcional).
        /// </summary>
        public Guid? UsuarioId { get; set; }
    }
}
