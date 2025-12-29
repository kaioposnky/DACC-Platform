using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DaccApi.Model.Responses.Order;

namespace DaccApi.Model.Objects.Order
{
    /// <summary>
    /// Representa um pedido no sistema.
    /// </summary>
    [Table("pedido")]
    public class Order
    {
        /// <summary>
        /// Obtém ou define o ID do pedido.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o ID do usuário que fez o pedido.
        /// </summary>
        [Column("usuario_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Obtém ou define a data em que o pedido foi feito.
        /// </summary>
        [Column("data_pedido")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Obtém ou define o status atual do pedido.
        /// </summary>
        [Column("status_pedido")]
        public string? Status { get; set; }

        /// <summary>
        /// Obtém ou define o valor total do pedido.
        /// </summary>
        [Column("total_pedido")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Obtém ou define o ID de pagamento do Mercado Pago.
        /// </summary>
        [Column("mercadopago_pagamento_id")]
        public long? MercadoPagoPaymentId { get; set; }

        /// <summary>
        /// Obtém ou define o ID da preferência de pagamento do Mercado Pago.
        /// </summary>
        [Column("preference_id")]
        public string? PreferenceId { get; set; }

        /// <summary>
        /// Obtém ou define o método de pagamento utilizado.
        /// </summary>
        [Column("metodo_pagamento")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Obtém ou define a lista de itens do pedido.
        /// </summary>
        [NotMapped]
        public List<OrderItem> OrderItems { get; set; } = [];

        /// <summary>
        /// Id do cupom do pedido.
        /// </summary>
        [NotMapped]
        public Guid? CupomId { get; set; }
        
        /// <summary>
        /// Converte o objeto Order em um OrderResponse.
        /// </summary>
        public OrderResponse ToOrderResponse()
        {
            return new OrderResponse(this);
        }
    }
}