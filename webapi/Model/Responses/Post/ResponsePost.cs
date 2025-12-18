using System.Text.Json.Serialization;

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
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o título do post.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do post.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        [JsonPropertyName("authorId")]
        public Guid AuthorId { get; set; } // Assuming AuthorId from other backend entities if available
        /// <summary>
        /// Obtém ou define a data de criação do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização do post (frontend specific, a ser preenchido por serviço).
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Obtém ou define as tags do post (mapeado de string para List(string)).
        /// </summary>
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade Post.
        /// </summary>
        /// <param name="post">A entidade Post de origem.</param>
        public ResponsePost(Post post)
        {
            Id = post.Id;
            Title = post.Titulo;
            Content = post.Conteudo;
            Tags = post.Tags.ToList();
            AuthorId = post.AutorId ?? Guid.Empty;
            CreatedAt = post.DataCriacao;
            UpdatedAt = post.DataAtualizacao;

        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponsePost() {
            Tags = new List<string>();
        }
    }
}
