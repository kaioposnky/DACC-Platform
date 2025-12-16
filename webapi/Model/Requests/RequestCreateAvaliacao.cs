using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
        /// <summary>
        /// Representa a requisição para criar uma nova avaliação.
        /// </summary>
        public class RequestCreateAvaliacao
        { 
                /// <summary>
                /// Obtém ou define a nota da avaliação.
                /// </summary>
                [Range(1, 5, ErrorMessage = "A nota deve ser de 1 a 5!")]
                public double Nota { get; set; }
                /// <summary>
                /// Obtém ou define o comentário da avaliação.
                /// </summary>
                public string? Comentario { get; set; }
                /// <summary>
                /// Obtém ou define o ID do produto a ser avaliado.
                /// </summary>
                public Guid ProdutoId { get; set; }
        }
}