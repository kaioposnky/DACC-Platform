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
        public double Rating { get; set; }
        
        /// <summary>
        /// Obtém ou define o título da avaliação.
        /// </summary>
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string? Title { get; set; }
        
        /// <summary>
        /// Obtém ou define o novo comentário da avaliação.
        /// </summary>
        public string? Comment { get; set; }

    }
}