using DaccApi.Model.Requests;

namespace DaccApi.Model.Objects.Order
{
    /// <summary>
    /// Representa um item dentro de um pedido.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Obtém ou define o ID do item do pedido.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o ID do produto.
        /// </summary>
        public Guid ProdutoId { get; set; }
        /// <summary>
        /// Obtém ou define o ID da variação do produto.
        /// </summary>
        public Guid ProdutoVariacaoId { get; set; }
        /// <summary>
        /// Obtém ou define a quantidade deste item.
        /// </summary>
        public int Quantidade { get; set; }
        /// <summary>
        /// Obtém ou define o preço unitário do item no momento da compra.
        /// </summary>
        public decimal PrecoUnitario { get; set; }

        /// <summary>
        /// Converte uma lista de requisições de itens de pedido para uma lista de objetos OrderItem.
        /// </summary>
        public static List<OrderItem> FromRequestList(IEnumerable<OrderItemRequest> requestList)
        {
            // Seleciona todos os elementos passa eles para FromRequest e retorna o resultado adicionando eles na lista
            return requestList.Select(request => FromRequest(request)).ToList();
        }
        
        /// <summary>
        /// Converte uma requisição de item de pedido para um objeto OrderItem.
        /// </summary>
        public static OrderItem FromRequest(OrderItemRequest request, decimal precoUnitario = 0)
        {
            return new OrderItem()
            {
                Id = Guid.Empty,
                ProdutoVariacaoId = request.ProdutoVariacaoId,
                Quantidade = request.Quantidade,
                ProdutoId = request.ProdutoId,
                PrecoUnitario = precoUnitario
            };
        }
    }
}