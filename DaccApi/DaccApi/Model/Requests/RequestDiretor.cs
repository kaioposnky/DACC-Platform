using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestDiretor
    {
        public Guid Id { get; set; }
            public string? Nome { get; set; }
            public string? Descricao { get; set; }
            [ImageValidation]
            public string? ImagemUrl { get; set; }
            public Guid? UsuarioId { get; set; }
            public Guid? DiretoriaId { get; set; }
            public string? Email { get; set; }
            public string? GithubLink    { get; set; }
            public string? LinkedinLink   { get; set; }
    }
    
}
