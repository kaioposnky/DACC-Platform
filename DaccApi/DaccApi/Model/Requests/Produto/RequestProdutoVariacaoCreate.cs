using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestProdutoVariacaoCreate
    {
        [Required(ErrorMessage = "Nome da cor é obrigatório")]
        [ColorValidation]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Tamanho é obrigatório")]
        [AllowedValues("PP", "P", "M", "G", "GG", "XG", "Pequeno", "Medio", "Grande", 
            ErrorMessage = "Tamanho deve ser: PP, P, M, G, GG, XG, Pequeno, Medio ou Grande")]
        public string Tamanho { get; set; }

        [Range(0, 99999, ErrorMessage = "Estoque deve estar entre 0 e 99.999 unidades")]
        public int Estoque { get; set; } = 0;

        [Range(0, 999, ErrorMessage = "Ordem da variação deve estar entre 0 e 999")]
        public int OrdemVariacao { get; set; } = 0;
    }
}