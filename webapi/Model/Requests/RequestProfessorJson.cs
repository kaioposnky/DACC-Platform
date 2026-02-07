using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição JSON para criar ou atualizar um professor (sem upload de imagem).
    /// </summary>
    public class RequestProfessorJson
    {
        /// <summary>
        /// Nome do professor.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }

        /// <summary>
        /// Título acadêmico (Dr., Ms., etc).
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Title { get; set; }

        /// <summary>
        /// Cargo na instituição.
        /// </summary>
        [Required(ErrorMessage = "O cargo é obrigatório")]
        public string Position { get; set; }

        /// <summary>
        /// Especialização ou área de atuação.
        /// </summary>
        [Required(ErrorMessage = "A especialização é obrigatória")]
        public string Specialization { get; set; }

        /// <summary>
        /// URL da imagem de perfil (opcional, caso já tenha sido feito upload separadamente).
        /// </summary>
        public string? ImageUrl { get; set; }

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
        public Guid? UserId { get; set; }
    }
}
