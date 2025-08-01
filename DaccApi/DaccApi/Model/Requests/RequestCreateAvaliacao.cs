using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
        public class RequestCreateAvaliacao
        {


                [Range(1, 5, ErrorMessage = "A nota deve ser de 1 a 5!")]
                public double Nota { get; set; }

                public int UsuarioId { get; set; }
                public string? Comentario { get; set; }
                public Guid ProdutoId { get; set; }
                
                public DateTime DataPostada { get; set; }

        }
}