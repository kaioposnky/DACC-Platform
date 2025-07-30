using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestQueryProdutos
    {
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Page { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "Limite deve estar entre 1 e 100")]
        public int Limit { get; set; } = 16;
        
        [StringLength(200, ErrorMessage = "Termo de busca deve ter no máximo 200 caracteres")]
        public string? SearchPattern { get; set; }

        public string? Categoria { get; set; }

        public string? Subcategoria { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Preço mínimo deve ser maior ou igual a 0")]
        public double? MinPrice { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Preço máximo deve ser maior ou igual a 0")]
        public double? MaxPrice { get; set; }

        [AllowedValues("price-low", "price-high", "newest", "name")]
        public string? SortBy { get; set; } = "newest";
    }
}