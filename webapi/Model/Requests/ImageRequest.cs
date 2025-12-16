using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model.Requests
{
    /// <summary>
    /// Representa uma requisição de imagem.
    /// </summary>
    public class ImageRequest
    {
        /// <summary>
        /// Obtém ou define o arquivo de imagem.
        /// </summary>
        [ImageValidation]
        public IFormFile ImageFile { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        public string ImageAlt { get; set; }
    }
}