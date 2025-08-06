using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestUpdateUsuario
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Curso { get; set; }
        public string? Telefone { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool? InscritoNoticia { get; set; }
    }
}
