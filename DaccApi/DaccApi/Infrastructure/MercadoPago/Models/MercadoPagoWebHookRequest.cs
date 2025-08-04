namespace DaccApi.Infrastructure.MercadoPago.Models
{
    public class MercadoPagoWebHookRequest
    {
        public string Action { get; set; }
        public WebhookData Data { get; set; }
        public DateTime DateCreated { get; set; }
        public long Id { get; set; }
        public bool LiveMode { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; } 
    }
    
    public class WebhookData
    {
        public string Id { get; set; }  // Este é o PaymentId
    }
}