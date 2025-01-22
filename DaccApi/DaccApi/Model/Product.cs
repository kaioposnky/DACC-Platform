namespace DaccApi.Model
{
    public class Product
    {
        public string Name { get; set; }
        public byte[]? ImageUrl { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public int? Id { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
