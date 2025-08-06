using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestNoticia
    {

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? Conteudo { get; set; }
        [ImageValidation]
        public string? ImagemUrl { get; set; }
        public string? Categoria { get; set; }
        
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        
        
    }
}