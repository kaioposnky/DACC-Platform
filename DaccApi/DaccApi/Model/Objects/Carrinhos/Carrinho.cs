namespace DaccApi.Model
{
    public class Carrinho
    {
        public Guid? Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public StatusCarrinho Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}