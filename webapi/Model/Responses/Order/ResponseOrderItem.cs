namespace DaccApi.Model.Responses.Order
{
    using DaccApi.Model.Objects.Order;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Representa a resposta de um item de pedido, adaptada para o frontend CartItem.
    /// </summary>
    public class ResponseOrderItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }

        [JsonPropertyName("productVariationId")]
        public Guid ProductVariationId { get; set; }

        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("productImage")]
        public string? ProductImage { get; set; }

        [JsonPropertyName("variationSize")]
        public string? VariationSize { get; set; }

        [JsonPropertyName("variationColor")]
        public string? VariationColor { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade OrderItem.
        /// </summary>
        /// <param name="orderItem">A entidade OrderItem de origem.</param>
        public ResponseOrderItem(OrderItem orderItem)
        {
            Id = orderItem.Id;
            OrderId = orderItem.OrderId;
            ProductId = orderItem.ProdutoId;
            ProductVariationId = orderItem.ProdutoVariacaoId;
            UnitPrice = orderItem.PrecoUnitario;
            Quantity = orderItem.Quantidade;
        }

        public ResponseOrderItem() { }
    }
}
