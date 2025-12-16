namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para uma avaliação.
    /// </summary>
    public class RequestAvaliacao
    
    {
        /// <summary>
        /// Obtém ou define o ID da avaliação.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define a nota da avaliação.
        /// </summary>
        public double Nota { get; set; }
        /// <summary>
        /// Obtém ou define o comentário da avaliação.
        /// </summary>
        public string? Comentario { get; set; }
        /// <summary>
        /// Obtém ou define o ID do produto avaliado.
        /// </summary>
        public Guid ProdutoId { get; set; }
    }
}
