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
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        [MinLength(10, ErrorMessage = "Descrição deve ter pelo menos 10 caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// Obtém ou define a categoria do produto.
        /// </summary>
        [Required(ErrorMessage = "Categoria é obrigatória")]
        // Recebe o ID como string
        public string Category { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria do produto.
        /// </summary>
        [Required(ErrorMessage = "Subcategoria é obrigatória")]
        // Recebe o ID como string
        public string Subcategory { get; set; }

        /// <summary>
        /// Obtém ou define o preço do produto.
        /// </summary>
        [Required(ErrorMessage = "Preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public double? Price { get; set; }
        
        public string? DetailedDescription { get; set; }
        
        public List<string>? PerfectFor { get; set; }
        
        public bool Featured { get; set; }
        
        public List<SpecificationItem>? Specifications { get; set; }
        
        public ShippingInfo? ShippingInfo { get; set; }

        /// <summary>
        /// Obtém ou define o preço original do produto (preço antes de desconto).
        /// </summary>
        public double? OriginalPrice { get; set; }

        /// <summary>
        /// Obtém ou define se o produto está em estoque.
        /// </summary>
        public bool? InStock { get; set; }
    }
}
