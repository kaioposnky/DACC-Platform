using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    public class ProdutoImagem
    {
        public Guid Id { get; set; }
        
        public Guid ProdutoVariacaoId { get; set; }
        
        [Required]
        [Url]
        public string ImagemUrl { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? ImagemAlt { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Ordem { get; set; }
    }
}