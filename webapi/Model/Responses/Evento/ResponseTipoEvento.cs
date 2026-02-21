namespace DaccApi.Model.Responses.Evento
{
    public class ResponseTipoEvento
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ResponseTipoEvento(TipoEvento entity)
        {
            Id = entity.Id;
            Name = entity.Nome;
        }
    }
}
