using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para atualizar uma variação de produto.
    /// </summary>
    public class RequestUpdateProdutoVariacao
    {
        /// <summary>
        /// Obtém ou define a nova cor da variação.
        /// </summary>
        [ColorValidation]
        public string? Cor { get; set; }

        /// <summary>
        /// Obtém ou define o novo tamanho da variação.
        /// </summary>
        [AllowedValues("PP", "P", "M", "G", "GG", "XG", "Pequeno", "Medio", "Grande", 
            ErrorMessage = "Tamanho deve ser: PP, P, M, G, GG, XG, Pequeno, Medio ou Grande")]
        public string? Tamanho { get; set; }

        /// <summary>
        /// Obtém ou define a nova quantidade em estoque.
        /// </summary>
        [Range(0, 99999, ErrorMessage = "Estoque deve estar entre 0 e 99.999 unidades")]
        public int? Estoque { get; set; }

        /// <summary>
        /// Obtém ou define a nova ordem de exibição da variação.
        /// </summary>
        [Range(0, 999, ErrorMessage = "Ordem da variação deve estar entre 0 e 999")]
        public int? OrdemVariacao { get; set; }
    }
}