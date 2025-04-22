namespace DaccApi.Model
{
    public class AvaliacaoProduto
    {
        public double Rating { get; set; }
        public Guid UserId { get; set; }
        public string? Commentary { get; set; }
        public Guid ProductId { get; set; }
        public bool State { get; set; } 
        public DateTime DatePosted { get; set; }
    }
}
