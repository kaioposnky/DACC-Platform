namespace DaccApi.Model
{
    public class Noticia
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public byte[] ImageUrl { get; set; }
    }
}

