using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestProjeto
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        [ImageValidation]
        
        public string? ImagemUrl { get; set; }
        
        public string? Status { get; set; }
        
        public string? Diretoria { get; set; }
        public string[]? Tags { get; set; }
    }
}
