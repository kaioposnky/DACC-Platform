namespace DaccApi.Infrastructure.MercadoPago.Models
{
    /// <summary>
    /// Representa a resposta da criação de uma preferência de pagamento.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Obtém ou define o ID da preferência de pagamento gerada.
        /// </summary>
        public required string PreferenceId { get; set; }
        /// <summary>
        /// Obtém ou define a URL de pagamento para redirecionamento.
        /// </summary>
        public required string PaymentUrl { get; set; } 
        /// <summary>
        /// Obtém ou define o status atual do pagamento.
        /// </summary>
        public required string Status { get; set; } 
    }
}