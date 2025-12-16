using System.Text.Json.Serialization;

namespace DaccApi.Infrastructure.MercadoPago.Models
{
    /// <summary>
    /// Representa a estrutura de uma requisição de webhook do Mercado Pago.
    /// </summary>
    public class MercadoPagoWebHookRequest
    {
        /// <summary>
        /// Obtém ou define a ação que disparou o webhook.
        /// </summary>
        [JsonPropertyName("action")]
        public string? Action { get; set; }
    
        /// <summary>
        /// Obtém ou define os dados específicos do evento de webhook.
        /// </summary>
        [JsonPropertyName("data")]
        public WebhookData? Data { get; set; }
    
        /// <summary>
        /// Obtém ou define a data de criação do evento.
        /// </summary>
        [JsonPropertyName("date_created")]
        public DateTime? DateCreated { get; set; }
    
        /// <summary>
        /// Obtém ou define o ID do evento.
        /// </summary>
        [JsonPropertyName("id")]
        public long? Id { get; set; }
    
        /// <summary>
        /// Obtém ou define se o evento ocorreu em modo de produção.
        /// </summary>
        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }
    
        /// <summary>
        /// Obtém ou define o tipo do evento (ex: "payment").
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    
        /// <summary>
        /// Obtém ou define o ID do usuário associado ao evento.
        /// </summary>
        [JsonPropertyName("user_id")]
        public object? UserId { get; set; } // Aceita string ou number
    
        /// <summary>
        /// Obtém ou define a versão da API que enviou o webhook.
        /// </summary>
        [JsonPropertyName("api_version")]
        public string? ApiVersion { get; set; }
    
        /// <summary>
        /// Obtém ou define o recurso que foi modificado.
        /// </summary>
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }
    
        /// <summary>
        /// Obtém ou define o tópico do evento.
        /// </summary>
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }
    }
    
    /// <summary>
    /// Representa o objeto de dados dentro de um webhook do Mercado Pago.
    /// </summary>
    public class WebhookData
    {
        /// <summary>
        /// Obtém ou define o ID do recurso (ex: ID do pagamento).
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }  // Este é o PaymentId
    }
}