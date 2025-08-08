using System.Text.Json.Serialization;

namespace DaccApi.Infrastructure.MercadoPago.Models
{
    public class MercadoPagoWebHookRequest
    {
        [JsonPropertyName("action")]
        public string? Action { get; set; }
    
        [JsonPropertyName("data")]
        public WebhookData? Data { get; set; }
    
        [JsonPropertyName("date_created")]
        public DateTime? DateCreated { get; set; }
    
        [JsonPropertyName("id")]
        public long? Id { get; set; }
    
        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }
    
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    
        [JsonPropertyName("user_id")]
        public object? UserId { get; set; } // Aceita string ou number
    
        [JsonPropertyName("api_version")]
        public string? ApiVersion { get; set; }
    
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }
    
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }
    }
    
    public class WebhookData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }  // Este é o PaymentId
    }
}