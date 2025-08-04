namespace DaccApi.Infrastructure.MercadoPago.Models
{
    public class PaymentResponse
    {
        public Guid PreferenceId { get; set; }
        public string PaymentUrl { get; set; } 
        public string Status { get; set; } 
    }
}