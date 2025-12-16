namespace DaccApi.Model.Responses
{
    /// <summary>
    /// Representa a resposta de um usuário.
    /// </summary>
    public class ResponseUsuario
    {
        /// <summary>
        /// Obtém ou define o ID do usuário.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do usuário.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtém ou define o sobrenome do usuário.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Obtém ou define o RA do usuário.
        /// </summary>
        public string? Ra { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail do usuário.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtém ou define o curso do usuário.
        /// </summary>
        public string? Course { get; set; }

        /// <summary>
        /// Obtém ou define o telefone do usuário.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem de perfil do usuário (avatar).
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Obtém ou define se o usuário está ativo.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Obtém ou define se o usuário está inscrito na newsletter.
        /// </summary>
        public bool? IsSubscribedToNews { get; set; }

        /// <summary>
        /// Obtém ou define o cargo do usuário.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Obtém ou define se o usuário está logado (frontend specific).
        /// </summary>
        public bool IsLoggedIn { get; set; } // Frontend specific property

        /// <summary>
        /// Obtém ou define a data de criação da conta.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade Usuario.
        /// </summary>
        /// <param name="usuario">A entidade Usuario de origem.</param>
        /// <param name="isLoggedIn">Indica se o usuário está logado (frontend specific).</param>
        public ResponseUsuario(Usuario usuario, bool isLoggedIn = false)
        {
            Id = usuario.Id;
            Name = usuario.Nome;
            LastName = usuario.Sobrenome;
            Ra = usuario.Ra;
            Email = usuario.Email;
            Course = usuario.Curso;
            Phone = usuario.Telefone;
            Avatar = usuario.ImagemUrl;
            IsActive = usuario.Ativo;
            IsSubscribedToNews = usuario.InscritoNoticia;
            Role = usuario.Cargo;
            CreatedAt = usuario.DataCriacao;
            UpdatedAt = usuario.DataAtualizacao;
            IsLoggedIn = isLoggedIn; // Propriedade específica do frontend
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponseUsuario()
        {
        }
    }
}