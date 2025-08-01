using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    public class ProdutoVariacao
    {
        public Guid Id { get; set; }
        
        public Guid ProdutoId { get; set; }
        
        public int CorId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Cor { get; set; } = string.Empty;
        
        public int TamanhoId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Tamanho { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue)]
        public int Estoque { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Sku { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue)]
        public int Ordem { get; set; }
        
        public List<ProdutoImagem> Imagens { get; set; } = new();
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public DateTime? DataAtualizacao { get; set; }
        
        public bool EmEstoque => Estoque > 0;

        public static string GenerateSku(Guid productId, string cor, string tamanho)
        {
            return $"PRD-{productId.ToString()[..8]}-{cor?.ToUpper()}-{tamanho?.ToUpper()}";
        }
        
        public static ProdutoVariacao FromRequest(RequestProdutoVariacaoCreate request, Guid productId, Guid variationId, string sku)
        {
            return new ProdutoVariacao
            {
                Id = variationId,
                ProdutoId = productId,
                Cor = request.Cor.Trim(),
                Tamanho = request.Tamanho,
                Estoque = request.Estoque,
                Ordem = request.OrdemVariacao,
                Sku = sku,
                DataCriacao = DateTime.UtcNow
            };
        }

        public void UpdateFromRequest(RequestUpdateProdutoVariacao request)
        {
            if (request.Cor != null) Cor = request.Cor.Trim();
            if (request.Tamanho != null) Tamanho = request.Tamanho;
            if (request.Estoque.HasValue) Estoque = request.Estoque.Value;
            if (request.OrdemVariacao.HasValue) Ordem = request.OrdemVariacao.Value;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}