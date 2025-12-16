using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa os parâmetros de consulta para busca de produtos.
    /// </summary>
    public class RequestQueryProdutos
    {
        /// <summary>
        /// Obtém ou define o número da página.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Pagina { get; set; } = 1;
        
        /// <summary>
        /// Obtém ou define o limite de itens por página.
        /// </summary>
        [Range(1, 100, ErrorMessage = "Limite deve estar entre 1 e 100")]
        public int Limite { get; set; } = 16;
        
        /// <summary>
        /// Obtém ou define o termo de pesquisa.
        /// </summary>
        [StringLength(200, ErrorMessage = "Termo de busca deve ter no máximo 200 caracteres")]
        public string? Pesquisa { get; set; }

        /// <summary>
        /// Obtém ou define a categoria para filtro.
        /// </summary>
        public string? Categoria { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria para filtro.
        /// </summary>
        public string? Subcategoria { get; set; }
        
        /// <summary>
        /// Obtém ou define o preço mínimo para filtro.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Preço mínimo deve ser maior ou igual a 0")]
        public double? PrecoMinimo { get; set; }
        
        /// <summary>
        /// Obtém ou define o preço máximo para filtro.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Preço máximo deve ser maior ou igual a 0")]
        public double? PrecoMaximo { get; set; }

        /// <summary>
        /// Obtém ou define o critério de ordenação.
        /// </summary>
        [AllowedValues("price-low", "price-high", "newest", "name")]
        public string? OrdenarPor { get; set; } = "newest";
    }
}