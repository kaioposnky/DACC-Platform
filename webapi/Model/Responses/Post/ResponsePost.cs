namespace DaccApi.Model.Responses.Post
{
    using DaccApi.Model.Post;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Representa a resposta de um post, adaptada para o frontend.
    /// </summary>
    public class ResponsePost
    {
        /// <summary>
        /// Obtém ou define o ID do post.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o título do post.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do post.
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public Guid AuthorId { get; set; } // Assuming AuthorId from other backend entities if available
        /// <summary>
        /// Obtém ou define a data de criação do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Obtém ou define as tags do post (mapeado de string para List(string)).
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade Post.
        /// </summary>
        /// <param name="post">A entidade Post de origem.</param>
        public ResponsePost(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            Tags = post.Tags?.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new List<string>();

            // Propriedades específicas do frontend sem correspondência direta em Post
            // Devem ser preenchidas por um serviço ou lógica adicional
            AuthorId = Guid.Empty; // Valor padrão
            CreatedAt = DateTime.UtcNow; // Valor padrão
            UpdatedAt = DateTime.UtcNow; // Valor padrão
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponsePost() {
            Tags = new List<string>();
        }
    }
}
