namespace DaccApi.Model
{
    public class RequestEvento
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
    
        public DateTime? Data { get; set; }
        public string? TipoEvento { get; set; }
        public Guid? AutorId { get; set; }
        public string? TextoAcao { get; set; }
        public string? LinkAcao { get; set; }

    }
}