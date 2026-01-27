using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um anúncio.
    /// </summary>
    public class RequestAnuncio
    {
        
        /// <summary>
        /// Obtém ou define o título do anúncio.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do anúncio.
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do anúncio.
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// Obtém ou define se o anúncio está ativo.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão primário.
        /// </summary>
        public string? PrimaryButtonText { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão primário.
        /// </summary>
        public string? PrimaryButtonLink { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão secundário.
        /// </summary>
        public string? SecondaryButtonText { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão secundário.
        /// </summary>
        public string? SecondaryButtonLink { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem (caso já hospedada).
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImageAlt { get; set; }
    }
}