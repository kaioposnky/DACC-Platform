namespace DaccApi.Infrastructure.MercadoPago.Models
{
    public class PaymentResponse
    {
        public required string PreferenceId { get; set; }
        public required string PaymentUrl { get; set; } 
        public required string Status { get; set; } 
    }
}