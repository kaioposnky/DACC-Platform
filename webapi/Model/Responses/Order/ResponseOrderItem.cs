namespace DaccApi.Model.Responses.Order
{
    using DaccApi.Model.Objects.Order;

    /// <summary>
    /// Representa a resposta de um item de pedido, adaptada para o frontend CartItem.
    /// </summary>
    public class ResponseOrderItem
    {
        /// <summary>
        /// Obtém ou define o ID do item do pedido.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o ID do produto.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Obtém ou define o ID da variação do produto.
        /// </summary>
        public Guid ProductVariationId { get; set; }

        /// <summary>
        /// Obtém ou define o nome do produto (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtém ou define o preço unitário do item.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade deste item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem do produto (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Obtém ou define o tamanho selecionado (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public string? SelectedSize { get; set; }

        /// <summary>
        /// Obtém ou define a cor selecionada (frontend specific, a ser preenchido por serviço).
        /// </summary>
        public string? SelectedColor { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade OrderItem.
        /// </summary>
        /// <param name="orderItem">A entidade OrderItem de origem.</param>
        public ResponseOrderItem(OrderItem orderItem)
        {
            Id = orderItem.Id;
            ProductId = orderItem.ProdutoId;
            ProductVariationId = orderItem.ProdutoVariacaoId;
            UnitPrice = orderItem.PrecoUnitario;
            Quantity = orderItem.Quantidade;

            // Propriedades específicas do frontend sem correspondência direta em OrderItem
            // Devem ser preenchidas por um serviço que consulta detalhes do produto/variação
            Name = null;
            Image = null;
            SelectedSize = null;
            SelectedColor = null;
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponseOrderItem()
        {
        }
    }
}
