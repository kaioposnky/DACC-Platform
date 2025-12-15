namespace DaccApi.Model
{
    public class Projeto
    {
        
        public Guid Id { get; set; }
        
        public string? Titulo { get; set; }
        
        public string? Descricao { get; set; }
        
        public string? ImagemUrl { get; set; }
        public string? ImagemAlt { get; set; }
        
        public string? Status { get; set; }
        
        public string? Diretoria { get; set; }
        
        public string[]? Tags { get; set; }

    }
}
