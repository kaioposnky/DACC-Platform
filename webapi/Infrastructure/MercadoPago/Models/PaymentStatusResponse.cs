namespace DaccApi.Infrastructure.MercadoPago.Models
{
    /// <summary>
    /// Representa a resposta detalhada do status de um pagamento.
    /// </summary>
    public class PaymentStatusResponse
    {
        /// <summary>
        /// Obtém ou define o ID do pagamento.
        /// </summary>
        public long PaymentId { get; set; }
        /// <summary>
        /// Obtém ou define o status do pagamento.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Obtém ou define o método de pagamento utilizado.
        /// </summary>
        public string? PaymentMethod { get; set; }
        /// <summary>
        /// Obtém ou define o valor da transação.
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// Obtém ou define a referência externa (geralmente o ID do pedido).
        /// </summary>
        public Guid ExternalReference { get; set; }
        /// <summary>
        /// Obtém ou define a data de criação do pagamento.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Obtém ou define a data de aprovação do pagamento.
        /// </summary>
        public DateTime? DateApproved { get; set; }
        /// <summary>
        /// Obtém ou define a data de expiração do pagamento.
        /// </summary>
        public DateTime? DateExpiration { get; set; }
    }
}