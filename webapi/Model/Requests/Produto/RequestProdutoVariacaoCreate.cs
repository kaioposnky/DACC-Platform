using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar uma nova variação de produto.
    /// </summary>
    public class RequestProdutoVariacaoCreate
    {
        /// <summary>
        /// Obtém ou define a cor da variação.
        /// </summary>
        [Required(ErrorMessage = "Nome da cor é obrigatório")]
        [ColorValidation]
        public string Color { get; set; }

        /// <summary>
        /// Obtém ou define o tamanho da variação.
        /// </summary>
        [Required(ErrorMessage = "Tamanho é obrigatório")]
        [AllowedValues("PP", "P", "M", "G", "GG", "XG", "Pequeno", "Medio", "Grande", 
            ErrorMessage = "Tamanho deve ser: PP, P, M, G, GG, XG, Pequeno, Medio ou Grande")]
        public string Size { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade em estoque inicial.
        /// </summary>
        [Range(0, 99999, ErrorMessage = "Estoque deve estar entre 0 e 99.999 unidades")]
        public int Stock { get; set; } = 0;

        /// <summary>
        /// Obtém ou define a ordem de exibição da variação.
        /// </summary>
        [Range(0, 999, ErrorMessage = "Ordem da variação deve estar entre 0 e 999")]
        public int DisplayOrder { get; set; } = 0;
    }
}