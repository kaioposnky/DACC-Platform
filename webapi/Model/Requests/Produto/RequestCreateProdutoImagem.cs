using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar uma nova imagem de produto.
    /// </summary>
    public class RequestCreateProdutoImagem
    {
        /// <summary>
        /// Obtém ou define a URL ou string Base64 da imagem.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [StringLength(255, ErrorMessage = "Texto alternativo deve ter no máximo 255 caracteres")]
        public string? ImageAlt { get; set; }

        /// <summary>
        /// Obtém ou define a ordem de exibição da imagem.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Ordem deve ser um valor positivo")]
        public int Order { get; set; } = 0;
    }
}