namespace DaccApi.Model
{
    /// <summary>
    /// Fornece informações detalhadas sobre uma variação de produto.
    /// </summary>
    public class ProdutoVariacaoInfo
    {
        /// <summary>
        /// Obtém ou define o ID da variação.
        /// </summary>
        public Guid VariationId { get; set; }
        /// <summary>
        /// Obtém ou define o ID do produto pai.
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Obtém ou define o nome da cor.
        /// </summary>
        public string? ColorName { get; set; }
        /// <summary>
        /// Obtém ou define o nome do tamanho.
        /// </summary>
        public string? SizeName { get; set; }
        /// <summary>
        /// Obtém ou define o preço da variação.
        /// </summary>
        public decimal Preco { get; set; }
        /// <summary>
        /// Obtém ou define o estoque físico total.
        /// </summary>
        public int Stock { get; set; } // Estoque físico total
        /// <summary>
        /// Obtém ou define a quantidade reservada em pedidos.
        /// </summary>
        public int ReservedStock { get; set; } // Quantidade reservada (nos pedidos)
        /// <summary>
        /// Obtém ou define o estoque disponível (físico - reservado).
        /// </summary>
        public int AvailableStock { get; set; } // Estoque disponível (físico - reservado)
        /// <summary>
        /// Obtém ou define a URL da imagem principal.
        /// </summary>
        public string? ImageUrl { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImageAlt { get; set; }
    }
}