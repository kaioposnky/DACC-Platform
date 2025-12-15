namespace DaccApi.Infrastructure.MercadoPago.Models
{
    public class PaymentStatusResponse
    {
        public long PaymentId { get; set; }
        public string Status { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal TransactionAmount { get; set; }
        public Guid ExternalReference { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? DateExpiration { get; set; }
    }
}