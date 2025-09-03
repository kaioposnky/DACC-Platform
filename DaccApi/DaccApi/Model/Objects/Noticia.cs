namespace DaccApi.Model
{
    public class Noticia
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public string? Conteudo { get; set; }
        public string? ImagemUrl { get; set; }
        public string? ImagemAlt { get; set; }
        public Guid? AutorId { get; set; }
        public string? Categoria { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
    }
}

