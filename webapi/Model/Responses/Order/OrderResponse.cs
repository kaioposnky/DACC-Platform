namespace DaccApi.Model.Responses.Order
{
    using DaccApi.Model.Objects.Order; 
    using System.Linq;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Representa a resposta de um pedido.
    /// </summary>
    public class OrderResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("mercadopagoPaymentId")]
        public long? MercadoPagoPaymentId { get; set; }

        [JsonPropertyName("preferenceId")]
        public string? PreferenceId { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("cupomId")]
        public Guid? CupomId { get; set; }

        [JsonPropertyName("items")]
        public List<ResponseOrderItem>? Items { get; set; }

        [JsonPropertyName("user")]
        public ResponseUsuario? User { get; set; }

        [JsonPropertyName("coupon")]
        public ResponseCupom? Coupon { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade Order.
        /// </summary>
        /// <param name="order">A entidade Order de origem.</param>
        public OrderResponse(Order order)
        {
            Id = order.Id;
            UserId = order.UserId;
            OrderDate = order.OrderDate;
            Status = order.Status;
            MercadoPagoPaymentId = order.MercadoPagoPaymentId;
            PreferenceId = order.PreferenceId;
            PaymentMethod = order.PaymentMethod;
            TotalAmount = order.TotalAmount;
            CupomId = order.CupomId;
            Items = order.OrderItems?.Select(item => new ResponseOrderItem(item)).ToList() ?? new List<ResponseOrderItem>();
        }

        public OrderResponse() { }
    }
}

