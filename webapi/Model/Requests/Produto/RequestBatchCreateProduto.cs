using DaccApi.Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Request para criação completa de produto com variações, imagens e especificações.
    /// </summary>
    public class RequestBatchCreateProduto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? DetailedDescription { get; set; }

        public string? Subcategory { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public double Price { get; set; }

        public double? OriginalPrice { get; set; }

        public bool? Featured { get; set; }

        public bool? Active { get; set; }

        public List<string>? PerfectFor { get; set; }

        /// <summary>
        /// Especificações técnicas (ex: Material, Gola, etc)
        /// </summary>
        public List<SpecificationItemRequest>? Specifications { get; set; }

        public ShippingInfoRequest? ShippingInfo { get; set; }

        /// <summary>
        /// Variações dinâmicas de estoque e SKU
        /// </summary>
        public List<VariationCreateRequest>? Variations { get; set; }
    }

    /// <summary>
    /// Request para criação de variação (sem ID, pois será gerado no servidor)
    /// </summary>
    public class VariationCreateRequest
    {
        [Required(ErrorMessage = "A cor é obrigatória")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tamanho é obrigatório")]
        public string Size { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser maior ou igual a zero")]
        public int Stock { get; set; }

        public string? SKU { get; set; }

        /// <summary>
        /// Lista detalhada de imagens, permitindo definir a Ordem explicitamente.
        /// </summary>
        public List<VariationImageCreateRequest>? Images { get; set; }
    }

    /// <summary>
    /// Representa a imagem de uma variação na criação (sem ID)
    /// </summary>
    public class VariationImageCreateRequest
    {
        /// <summary>
        /// A URL da imagem (já retornada pelo upload).
        /// </summary>
        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// A posição exata em que a imagem deve aparecer na galeria.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "A ordem deve ser maior que zero")]
        public int Order { get; set; }

        /// <summary>
        /// Texto alternativo (opcional).
        /// </summary>
        public string? ImageAlt { get; set; }
    }
}
