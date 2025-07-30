namespace DaccApi.Model
{
    public class RequestAvaliacao
    {
        public double Nota { get; set; }
        public int UsuarioId { get; set; }
        public string? Comentario { get; set; }
        public Guid ProdutoId { get; set; }
    }
}
