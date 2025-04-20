namespace DaccApi.Model
{
    public class ProductRating
    {
        public double Rating { get; set; }
        public string? Commentary { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int State { get; set; } // Estado de ativação da avaliação, 0 = desativada, 1 = ativada
        public DateTime DatePosted { get; set; }
    }
}
