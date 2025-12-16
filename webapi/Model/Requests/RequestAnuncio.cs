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
    }
}