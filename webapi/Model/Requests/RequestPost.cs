namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um post.
    /// </summary>
    public class RequestPost
    {
        /// <summary>
        /// Obtém ou define o título do post.
        /// </summary>
        public string? Titulo { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do post.
        /// </summary>
        public string? Conteudo { get; set; }
        /// <summary>
        /// Obtém ou define as tags do post.
        /// </summary>
        public string[]? Tags { get; set; }
    }
}