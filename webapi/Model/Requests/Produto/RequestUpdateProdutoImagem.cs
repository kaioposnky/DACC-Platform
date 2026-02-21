using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para atualizar uma imagem de produto.
    /// </summary>
    public class RequestUpdateProdutoImagem
    {


        /// <summary>
        /// Obtém ou define a nova URL da imagem (caso já hospedada).
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Obtém ou define o novo texto alternativo da imagem.
        /// </summary>
        [StringLength(255, ErrorMessage = "Texto alternativo deve ter no máximo 255 caracteres")]
        public string? ImageAlt { get; set; }

        /// <summary>
        /// Obtém ou define a nova ordem de exibição da imagem.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Ordem deve ser um valor positivo")]
        public int? Order { get; set; }
    }
}