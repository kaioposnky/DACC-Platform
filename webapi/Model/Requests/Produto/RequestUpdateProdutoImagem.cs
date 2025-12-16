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
        /// Obtém ou define o novo arquivo de imagem.
        /// </summary>
        [ImageValidation]
        public IFormFile? Imagem { get; set; }

        /// <summary>
        /// Obtém ou define o novo texto alternativo da imagem.
        /// </summary>
        [StringLength(255, ErrorMessage = "Texto alternativo deve ter no máximo 255 caracteres")]
        public string? ImagemAlt { get; set; }

        /// <summary>
        /// Obtém ou define a nova ordem de exibição da imagem.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Ordem deve ser um valor positivo")]
        public int? Ordem { get; set; }
    }
}