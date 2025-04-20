namespace DaccApi.Model
{
    public class RequestProduto
    {
        public string? Name { get; set; }
        public Guid? Id { get; set; }
        public double? Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double Rating { get; set; }
    }
}
