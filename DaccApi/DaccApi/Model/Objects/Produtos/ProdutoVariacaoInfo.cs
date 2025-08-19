namespace DaccApi.Model
{
    public class ProdutoVariacaoInfo
    {
        public Guid VariationId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ColorName { get; set; }
        public string? SizeName { get; set; }
        public decimal Preco { get; set; }
        public int Stock { get; set; } // Estoque físico total
        public int ReservedStock { get; set; } // Quantidade reservada (nos pedidos)
        public int AvailableStock { get; set; } // Estoque disponível (físico - reservado)
        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }
    }
}