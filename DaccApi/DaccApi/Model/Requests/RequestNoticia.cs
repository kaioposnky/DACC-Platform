namespace DaccApi.Model
{
    public class RequestNoticia
    {

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? Conteudo { get; set; }
        public string? ImagemUrl { get; set; }
        public int? Autor_Id { get; set; }
        public string? Categoria { get; set; }
        public DateTime Data_Atualizacao { get; set; }
        public DateTime Data_Publicacao { get; set; }
        
    }
}