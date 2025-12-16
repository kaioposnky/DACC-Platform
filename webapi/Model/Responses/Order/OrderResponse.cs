namespace DaccApi.Model.Responses.Order
{
    using DaccApi.Model.Objects.Order; 
    using System.Linq;

    /// <summary>
    /// Representa a resposta de um pedido.
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// Obtém ou define o ID do pedido.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o ID do usuário.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Obtém ou define a data do pedido.
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Obtém ou define o status do pedido.
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Obtém ou define o método de pagamento.
        /// </summary>
        public string? PaymentMethod { get; set; }
        /// <summary>
        /// Obtém ou define o valor total do pedido.
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Obtém ou define a lista de itens do pedido.
        /// </summary>
        public List<ResponseOrderItem>? OrderItems { get; set; }

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
            PaymentMethod = order.PaymentMethod;
            TotalAmount = order.TotalAmount;
            OrderItems = order.OrderItems?.Select(item => new ResponseOrderItem(item)).ToList() ?? new List<ResponseOrderItem>();
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public OrderResponse() { }
    }
}

