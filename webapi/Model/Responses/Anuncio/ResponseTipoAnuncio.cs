namespace DaccApi.Model.Responses.Anuncio
{
    public class ResponseTipoAnuncio
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ResponseTipoAnuncio(TipoAnuncio tipoAnuncio)
        {
            Id = tipoAnuncio.Id;
            Name = tipoAnuncio.Nome;
        }
    }
}
