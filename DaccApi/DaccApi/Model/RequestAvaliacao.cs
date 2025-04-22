namespace DaccApi.Model
{
    public class RequestAvaliacao
    {
        public double? Rating { get; set; }
        public Guid? UserId { get; set; }
        public string? Commentary { get; set; }
        public Guid? ProductId { get; set; }
    }
}
