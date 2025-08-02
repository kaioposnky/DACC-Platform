namespace DaccApi.Model
{
    public class AvaliacaoProduto
    {
        public Guid Id { get; set; }
        public double Nota { get; set; }
        public Guid UsuarioId { get; set; }
        public string? Comentario { get; set; }
        public Guid ProdutoId { get; set; }
        public DateTime DataPostada { get; set; }
    }
}
