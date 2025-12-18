using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma imagem associada a uma variação de produto.
    /// </summary>
    public class ProdutoImagem
    {
        /// <summary>
        /// Obtém ou define o ID da imagem.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Obtém ou define o ID da variação de produto à qual a imagem pertence.
        /// </summary>
        [Column("produto_variacao_id")]
        public Guid? ProdutoVariacaoId { get; set; }
        
        /// <summary>
        /// Obtém ou define a URL da imagem.
        /// </summary>
        [Required]
        [Url]
        public string ImagemUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Obtém ou define o texto alternativo para a imagem.
        /// </summary>
        [StringLength(255)]
        public string? ImagemAlt { get; set; }
        
        /// <summary>
        /// Obtém ou define a ordem de exibição da imagem.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Ordem { get; set; }
    }
}