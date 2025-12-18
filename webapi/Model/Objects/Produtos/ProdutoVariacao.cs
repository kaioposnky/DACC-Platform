using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma variação de um produto (ex: cor, tamanho).
    /// </summary>
    public class ProdutoVariacao
    {
        /// <summary>
        /// Obtém ou define o ID da variação.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Obtém ou define o ID do produto pai.
        /// </summary>
        [Column("produto_id")]
        public Guid? ProdutoId { get; set; }
        
        /// <summary>
        /// Obtém ou define o ID da cor.
        /// </summary>
        [Column("cor_id")]
        public Guid? CorId { get; set; }
        
        /// <summary>
        /// Obtém ou define o nome da cor.
        /// </summary>
        [Required]
        [StringLength(50)]
        [NotMapped]
        public string Cor { get; set; } = string.Empty;
        
        /// <summary>
        /// Obtém ou define o ID do tamanho.
        /// </summary>
        [Column("tamanho_id")]
        public Guid? TamanhoId { get; set; }
        
        /// <summary>
        /// Obtém ou define o nome do tamanho.
        /// </summary>
        [Required]
        [StringLength(20)]
        [NotMapped]
        public string Tamanho { get; set; } = string.Empty;
        
        /// <summary>
        /// Obtém ou define a quantidade em estoque.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Estoque { get; set; }
        
        /// <summary>
        /// Obtém ou define o SKU (Stock Keeping Unit) da variação.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Sku { get; set; } = string.Empty;
        
        /// <summary>
        /// Obtém ou define a ordem de exibição da variação.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Ordem { get; set; }
        
        /// <summary>
        /// Obtém ou define a lista de imagens da variação.
        /// </summary>
        public List<ProdutoImagem> Imagens { get; set; } = new();
        
        /// <summary>
        /// Obtém ou define a data de criação.
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
        
        /// <summary>
        /// Obtém um valor que indica se a variação está em estoque.
        /// </summary>
        public bool EmEstoque => Estoque > 0;

        /// <summary>
        /// Gera um SKU único para a variação de produto.
        /// </summary>
        public static string GenerateSku(Guid productId, string cor, string tamanho)
        {
            return $"PRD-{productId.ToString()[..8]}-{cor?.ToUpper()}-{tamanho?.ToUpper()}";
        }
        
        /// <summary>
        /// Cria um objeto ProdutoVariacao a partir de uma requisição de criação.
        /// </summary>
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

        /// <summary>
        /// Atualiza as propriedades da variação a partir de uma requisição de atualização.
        /// </summary>
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