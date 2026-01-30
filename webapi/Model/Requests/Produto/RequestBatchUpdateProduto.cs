using DaccApi.Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Request principal para atualização completa do produto.
    /// </summary>
    /// <summary>
    /// Request principal para atualização completa do produto.
    /// </summary>
    public class RequestBatchUpdateProduto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? DetailedDescription { get; set; }

        public string? Category { get; set; }

        public string? Subcategory { get; set; }

        public double Price { get; set; }

        public double? OriginalPrice { get; set; }

        public bool? Featured { get; set; }

        public bool? InStock { get; set; }

        public List<string>? PerfectFor { get; set; }

        /// <summary>
        /// Especificações técnicas (ex: Material, Gola, etc)
        /// </summary>
        public List<SpecificationItemRequest>? Specifications { get; set; }

        public ShippingInfoRequest? ShippingInfo { get; set; }

        /// <summary>
        /// Variações dinâmicas de estoque e SKU
        /// </summary>
        public List<VariationUpdateRequest>? Variations { get; set; }

    }

    public class VariationUpdateRequest
    {
        public Guid? Id { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string? SKU { get; set; }
        /// <summary>
        /// Lista detalhada de imagens, permitindo definir a Ordem explicitamente.
        /// </summary>
        public List<VariationImageRequest>? Images { get; set; }
    }

    /// <summary>
    /// Representa o vínculo da imagem com a variação, controlando a Ordem.
    /// </summary>
    public class VariationImageRequest
    {
        /// <summary>
        /// ID do registro da imagem (caso seja uma edição de imagem existente).
        /// Deixe nulo se for uma nova associação.
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// A URL da imagem (já retornada pelo upload).
        /// </summary>
        [Required]
        public string Url { get; set; } = string.Empty;
        /// <summary>
        /// A posição exata em que a imagem deve aparecer na galeria.
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Texto alternativo (opcional).
        /// </summary>
        public string? ImageAlt { get; set; }
    }

    public class SpecificationItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class ShippingInfoRequest
    {
        public bool FreeShipping { get; set; }
        public string EstimatedDays { get; set; } = string.Empty;
        public string ReturnPolicy { get; set; } = string.Empty;
        public string? Warranty { get; set; }
    }
}
