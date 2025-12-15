namespace DaccApi.Model.Responses;

public class ResponseAnuncio
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Conteudo { get; set; }
    public string? TipoAnuncio { get; set; }
    public string? ImagemUrl { get; set; }
    public string? ImagemAlt { get; set; }
    public bool Ativo { get; set; }
    public Guid AutorId { get; set; }
}