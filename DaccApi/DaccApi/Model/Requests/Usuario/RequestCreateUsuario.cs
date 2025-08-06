using DaccApi.Enum.UserEnum;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestCreateUsuario
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Ra { get; set; }
        public string? Curso { get; set; }
        public string? Telefone { get; set; }
        public string? Senha { get; set; }
        public bool? NewsLetterSubscriber { get; set; }

    }
}
