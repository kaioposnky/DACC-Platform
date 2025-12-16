using DaccApi.Model.Responses.Order;

namespace DaccApi.Model.Objects.Order
{
    /// <summary>
    /// Representa um pedido no sistema.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Obtém ou define o ID do pedido.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o ID do usuário que fez o pedido.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Obtém ou define a data em que o pedido foi feito.
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Obtém ou define o status atual do pedido.
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Obtém ou define o valor total do pedido.
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Obtém ou define o ID de pagamento do Mercado Pago.
        /// </summary>
        public long? MercadoPagoPaymentId { get; set; }
        /// <summary>
        /// Obtém ou define o ID da preferência de pagamento do Mercado Pago.
        /// </summary>
        public string? PreferenceId { get; set; }
        /// <summary>
        /// Obtém ou define o método de pagamento utilizado.
        /// </summary>
        public string? PaymentMethod { get; set; }
        /// <summary>
        /// Obtém ou define a lista de itens do pedido.
        /// </summary>
        public List<OrderItem> OrderItems { get; set; } = [];
        
        /// <summary>
        /// Converte o objeto Order em um OrderResponse.
        /// </summary>
        public OrderResponse ToOrderResponse()
        {
            return new OrderResponse
            {
                Id = Id,
                UserId = UserId,
                OrderDate = OrderDate,
                Status = Status,
                TotalAmount = TotalAmount,
                OrderItems = OrderItems
            };
        }
    }
}