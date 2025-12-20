using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar um novo produto.
    /// </summary>
    public class RequestCreateProduto
    {
        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        [MinLength(3, ErrorMessage = "Nome deve ter pelo menos 3 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        [MinLength(10, ErrorMessage = "Descrição deve ter pelo menos 10 caracteres")]
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define a categoria do produto.
        /// </summary>
        [Required(ErrorMessage = "Categoria é obrigatória")]
        // Recebe o ID como string
        public string Categoria { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria do produto.
        /// </summary>
        [Required(ErrorMessage = "Subcategoria é obrigatória")]
        // Recebe o ID como string
        public string Subcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o preço do produto.
        /// </summary>
        [Required(ErrorMessage = "Preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public double? Preco { get; set; }
        
        public string? DescricaoDetalhada { get; set; }
        
        public List<string>? PerfeitoPara { get; set; }
        
        public bool Destaque { get; set; }
        
        public List<SpecificationItem>? Especificacoes { get; set; }
        
        public ShippingInfo? InformacaoEnvio { get; set; }
    }
}
