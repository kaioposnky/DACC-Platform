using System.ComponentModel.DataAnnotations;
using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestUpdateUsuario
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Curso { get; set; }
        [Phone]
        public string? Telefone { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool? InscritoNoticia { get; set; }
    }
}
