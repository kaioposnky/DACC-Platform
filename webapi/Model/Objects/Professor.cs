using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model.Objects
{
    /// <summary>
    /// Representa um professor ou membro do corpo docente (faculty).
    /// </summary>
    [Table("professores")]
    public class Professor
    {
        /// <summary>
        /// Obtém ou define o ID do professor.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do professor.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define o título acadêmico (ex: Dr., Prof.).
        /// </summary>
        [Column("titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// Obtém ou define o cargo do professor.
        /// </summary>
        [Column("cargo")]
        public string Cargo { get; set; }

        /// <summary>
        /// Obtém ou define a especialização do professor.
        /// </summary>
        [Column("especializacao")]
        public string Especializacao { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem do professor.
        /// </summary>
        [Column("imagem_url")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail de contato do professor.
        /// </summary>
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Obtém ou define o link para o perfil do LinkedIn.
        /// </summary>
        [Column("linkedin")]
        public string? Linkedin { get; set; }

        /// <summary>
        /// Obtém ou define o link para o perfil do GitHub.
        /// </summary>
        [Column("github")]
        public string? Github { get; set; }

        /// <summary>
        /// Obtém ou define o ID do usuário associado.
        /// </summary>
        [Column("usuario_id")]
        public Guid? UserId { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [Column("data_atualizacao")]
        public DateTime DataAtualizacao { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }

        /// <summary>
        /// Cria uma instância de Professor a partir de um RequestProfessor.
        /// </summary>
        public static Professor FromRequest(RequestProfessor request)
        {
            return new Professor
            {
                Nome = request.Name,
                Titulo = request.Title,
                Cargo = request.Position,
                Especializacao = request.Specialization,
                Email = request.Email,
                Linkedin = request.Linkedin,
                Github = request.Github,
                UserId = request.UserId
            };
        }
    }
}
