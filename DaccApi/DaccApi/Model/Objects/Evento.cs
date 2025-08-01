namespace DaccApi.Model;

public class Evento
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime? Data { get; set; }
    public string? tipoEvento { get; set; }
    public int? AutorId { get; set; }
    public string? TextoAcao { get; set; }
    public string? LinkAcao { get; set; }
    
}