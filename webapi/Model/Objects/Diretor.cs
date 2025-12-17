using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model.Objects
{
    /// <summary>
    /// Representa um membro da diretoria do DACC.
    /// </summary>
    [Table("diretores")]
    public class Diretor
    {
        /// <summary>
        /// Obtém ou define o ID do diretor.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do diretor.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define o título acadêmico (ex: Dr., Prof.).
        /// </summary>
        [Column("titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// Obtém ou define o cargo do diretor.
        /// </summary>
        [Column("cargo")]
        public string Cargo { get; set; }

        /// <summary>
        /// Obtém ou define a especialização do diretor.
        /// </summary>
        [Column("especializacao")]
        public string Especializacao { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem do diretor.
        /// </summary>
        [Column("imagem_url")]
        public string ImagemUrl { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail de contato do diretor.
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
        public Guid? UsuarioId { get; set; }

        /// <summary>
        /// Cria uma instância de Diretor a partir de um RequestDiretor.
        /// </summary>
        public static Diretor FromRequest(RequestDiretor request, string imageUrl)
        {
            return new Diretor
            {
                Nome = request.Nome,
                Titulo = request.Titulo,
                Cargo = request.Cargo,
                Especializacao = request.Especializacao,
                ImagemUrl = imageUrl,
                Email = request.Email,
                Linkedin = request.Linkedin,
                Github = request.Github,
                UsuarioId = request.UsuarioId
            };
        }
    }
}
