using DaccApi.Enum.Produtos;

namespace DaccApi.Model.Responses
{
    public class ResponseProduto
    {
        public Guid Id { get; set; }
        
        public string Nome { get; set; } = string.Empty;
        
        public string Descricao { get; set; } = string.Empty;
        
        public double? Preco { get; set; }
        
        public string Categoria { get; set; } = string.Empty;
        
        public string Subcategoria { get; set; } = string.Empty;
        
        public List<ResponseProdutoVariacao> Variacoes { get; set; } = new();
        
        public DateTime? DataCriacao { get; set; }
        
        public DateTime? DataAtualizacao { get; set; }
    }
}