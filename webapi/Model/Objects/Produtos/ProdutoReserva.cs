namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma reserva de estoque para uma variação de produto em um pedido.
    /// </summary>
    public class ProdutoReserva
    {
        /// <summary>
        /// Obtém ou define o ID da reserva.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o ID da variação de produto reservada.
        /// </summary>
        public Guid ProductVariationId { get; set; }
        /// <summary>
        /// Obtém ou define o ID do pedido ao qual a reserva pertence.
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Obtém ou define a quantidade de itens reservados.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Obtém ou define a data e hora em que a reserva expira.
        /// </summary>
        public DateTime ExpiresAt { get; set; } 
        /// <summary>
        /// Obtém ou define se a reserva está ativa.
        /// </summary>
        public bool IsActive { get; set; }
    }
}