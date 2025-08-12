namespace DaccApi.Model
{
    public class Anuncio
    {

        public Guid Id  { get; set; }
        public string? Titulo  { get; set; }
        public string? Conteudo { get; set; }
        public string? TipoAnuncio { get; set; }
        public string? ImagemUrl { get; set; }
        public string? ImagemAlt { get; set; }
        public bool Ativo { get; set; }
        public Guid AutorId { get; set; }
        public DateTime? DataCriacao { get; set; }
    }
}