namespace DaccApi.Model
{
    public class RequestProjeto
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        
        public IFormFile? ImageFile { get; set; }
        
        public string? Status { get; set; }
        
        public string? Diretoria { get; set; }
        public string[]? Tags { get; set; }
    }
}
