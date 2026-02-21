using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Curso
{
    public class RequestUpdateCurso
    {
        [Required(ErrorMessage = "O nome do curso é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome do curso deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; }
    }
}
