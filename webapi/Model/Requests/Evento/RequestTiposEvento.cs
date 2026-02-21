using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Evento
{
    public class RequestCreateTipoEvento
    {
        [Required(ErrorMessage = "O nome do tipo de evento é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }

    public class RequestUpdateTipoEvento
    {
        [Required(ErrorMessage = "O nome do tipo de evento é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}
