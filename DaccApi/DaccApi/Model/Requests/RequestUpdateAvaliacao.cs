using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model

{
    
    public class RequestUpdateAvaliacao
    {
        [Range(1, 5, ErrorMessage = "A nota deve ser de 1 a 5!")]
        public double Nota { get; set; }
        public string? Comentario { get; set; }

    }
    
}