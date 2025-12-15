using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    public class RequestCreateProduto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        [MinLength(3, ErrorMessage = "Nome deve ter pelo menos 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        [MinLength(10, ErrorMessage = "Descrição deve ter pelo menos 10 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        [MinLength(3, ErrorMessage = "Categoria deve ter pelo menos 3 caracteres")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Subcategoria é obrigatória")]
        [MinLength(3, ErrorMessage = "Subcategoria deve ter pelo menos 3 caracteres")]
        public string Subcategoria { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public double? Preco { get; set; }
    }
}
