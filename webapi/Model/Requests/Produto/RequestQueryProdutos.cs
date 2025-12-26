using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Requests;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa os parâmetros de consulta para busca de produtos.
    /// </summary>
    public class RequestQueryProdutos : BaseQueryRequest
    {
        /// <summary>
        /// Obtém ou define a categoria para filtro.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Obtém ou define o preço mínimo para filtro.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Preço mínimo deve ser maior ou igual a 0")]
        public double? MinPrice { get; set; }
        
        /// <summary>
        /// Obtém ou define o preço máximo para filtro.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Preço máximo deve ser maior ou igual a 0")]
        public double? MaxPrice { get; set; }

        /// <summary>
        /// Obtém ou define o critério de ordenação.
        /// </summary>
        [AllowedValues("price-low", "price-high", "newest", "name")]
        public string? OrderBy { get; set; } = "newest";
    }
}