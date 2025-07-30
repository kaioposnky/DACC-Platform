namespace DaccApi.Model.Responses
{
    public class ResponseProdutoImagem
    {
        public Guid Id { get; set; }
        
        public Guid ProdutoVariacaoId { get; set; }
        
        public string ImagemUrl { get; set; } = string.Empty;
        public string? ImagemAlt { get; set; }
        public int Ordem { get; set; }
    }
}