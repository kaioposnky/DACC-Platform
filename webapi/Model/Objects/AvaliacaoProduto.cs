namespace DaccApi.Model
{
    /// <summary>
    /// Representa a avaliação de um produto feita por um usuário.
    /// </summary>
    public class AvaliacaoProduto
    {
        /// <summary>
        /// Obtém ou define o ID da avaliação.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define a nota da avaliação (ex: de 1 a 5).
        /// </summary>
        public double Nota { get; set; }
        /// <summary>
        /// Obtém ou define o ID do usuário que fez a avaliação.
        /// </summary>
        public Guid UsuarioId { get; set; }
        /// <summary>
        /// Obtém ou define o comentário da avaliação.
        /// </summary>
        public string? Comentario { get; set; }
        /// <summary>
        /// Obtém ou define o ID do produto avaliado.
        /// </summary>
        public Guid ProdutoId { get; set; }
        /// <summary>
        /// Obtém ou define a data em que a avaliação foi postada.
        /// </summary>
        public DateTime DataPostada { get; set; }
    }
}
