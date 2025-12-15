namespace DaccApi.Model
{
    public class ProdutoReserva
    {
        public Guid Id { get; set; }
        public Guid ProductVariationId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiresAt { get; set; } 
        public bool IsActive { get; set; }
    }
}