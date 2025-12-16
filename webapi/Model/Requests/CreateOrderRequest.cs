namespace DaccApi.Model.Requests
{
    /// <summary>
    /// Representa a requisição para criar um novo pedido.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Obtém ou define a lista de itens do pedido.
        /// </summary>
        public List<OrderItemRequest> ItensPedido { get; set; }
    }

    /// <summary>
    /// Representa um item dentro da requisição de criação de pedido.
    /// </summary>
    public class OrderItemRequest
    {
        /// <summary>
        /// Obtém ou define o ID do produto.
        /// </summary>
        public Guid ProdutoId { get; set; }
        /// <summary>
        /// Obtém ou define o ID da variação do produto.
        /// </summary>
        public Guid ProdutoVariacaoId { get; set; }
        /// <summary>
        /// Obtém ou define a quantidade do item.
        /// </summary>
        public int Quantidade { get; set; }
    }
}