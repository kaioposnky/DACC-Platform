using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para atualizar um produto.
    /// </summary>
    public class RequestUpdateProduto
    {
        /// <summary>
        /// Obtém ou define o novo nome do produto.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtém ou define a nova descrição do produto.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Obtém ou define a nova categoria do produto.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Obtém ou define a nova subcategoria do produto.
        /// </summary>
        public string? Subcategory { get; set; }

        /// <summary>
        /// Obtém ou define o novo preço do produto.
        /// </summary>
        public double? Price { get; set; }
        
        /// <summary>
        /// Obtém ou define o novo preço original do produto.
        /// </summary>
        public double? OriginalPrice { get; set; }
        
        public string? DetailedDescription { get; set; }
        
        public List<string>? PerfectFor { get; set; }
        
        public bool? Featured { get; set; }
        
        public List<SpecificationItem>? Specifications { get; set; }
        
        public ShippingInfo? ShippingInfo { get; set; }
    }
}
