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
        public string? Titulo { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do anúncio.
        /// </summary>
        public string? Conteudo { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do anúncio.
        /// </summary>
        public string? TipoAnuncio { get; set; }
        /// <summary>
        /// Obtém ou define se o anúncio está ativo.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão primário.
        /// </summary>
        public string? BotaoPrimarioTexto { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão primário.
        /// </summary>
        public string? BotaoPrimarioLink { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão secundário.
        /// </summary>
        public string? BotaoSecundarioTexto { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão secundário.
        /// </summary>
        public string? BotaoSecundarioLink { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem (caso já hospedada).
        /// </summary>
        public string? ImagemUrl { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImagemAlt { get; set; }
    }
}