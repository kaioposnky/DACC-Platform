using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model

{
    /// <summary>
    /// Representa a requisição para atualizar uma avaliação.
    /// </summary>
    public class RequestUpdateAvaliacao
    {
        /// <summary>
        /// Obtém ou define a nova nota da avaliação.
        /// </summary>
        [Range(1, 5, ErrorMessage = "A nota deve ser de 1 a 5!")]
        public double Nota { get; set; }
        /// <summary>
        /// Obtém ou define o novo comentário da avaliação.
        /// </summary>
        public string? Comentario { get; set; }

    }
}