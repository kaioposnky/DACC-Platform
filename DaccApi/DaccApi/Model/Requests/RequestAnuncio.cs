using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestAnuncio
    {
        
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }
        public string? TipoAnuncio { get; set; }
        [ImageValidation]
        public IFormFile ImageFile { get; set; }
        public string? ImagemAlt { get; set; }
        public bool Ativo { get; set; }
    }
}