namespace DaccApi.Model.Requests
{
    /// <summary>
    /// Representa a requisição para criar um novo pedido, alinhada com o carrinho do frontend.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Lista de itens do carrinho.
        /// </summary>
        public List<CartItemRequest> Items { get; set; }

        /// <summary>
        /// Código do cupom de desconto (opcional).
        /// </summary>
        public string? CouponCode { get; set; }
    }

    /// <summary>
    /// Representa um item do carrinho vindo do frontend.
    /// </summary>
    public class CartItemRequest
    {
        /// <summary>
        /// ID da variação do produto (no frontend é o 'id').
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID do produto pai.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantidade do item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
