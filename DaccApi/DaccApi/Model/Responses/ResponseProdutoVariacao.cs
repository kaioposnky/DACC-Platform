namespace DaccApi.Model.Responses
{
    public class ResponseProdutoVariacao
    {
        public Guid Id { get; set; }
        
        public Guid ProdutoId { get; set; }
        
        public string Cor { get; set; } = string.Empty;
        
        public string Tamanho { get; set; } = string.Empty;
        
        public int Estoque { get; set; }
        
        public string Sku { get; set; } = string.Empty;
        
        public int Ordem { get; set; }
        
        public List<ResponseProdutoImagem> Imagens { get; set; } = new();
        
        public DateTime DataCriacao { get; set; }
        
        public DateTime? DataAtualizacao { get; set; }
    }
}