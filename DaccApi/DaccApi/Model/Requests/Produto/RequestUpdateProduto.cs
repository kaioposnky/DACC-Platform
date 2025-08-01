namespace DaccApi.Model
{
    public class RequestUpdateProduto
    {
        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public string? Categoria { get; set; }

        public string? Subcategoria { get; set; }

        public double? Preco { get; set; }
        
        public double? PrecoOriginal { get; set; }
    }
}
