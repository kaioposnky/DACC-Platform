namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma notícia no sistema.
    /// </summary>
    public class Noticia
    {
        /// <summary>
        /// Obtém ou define o ID da notícia.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o título da notícia.
        /// </summary>
        public string? Titulo { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define a descrição da notícia.
        /// </summary>
        public string? Descricao { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define o conteúdo da notícia.
        /// </summary>
        public string? Conteudo { get; set; }
        /// <summary>
        /// Obtém ou define a URL da imagem da notícia.
        /// </summary>
        public string? ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImagemAlt { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor da notícia.
        /// </summary>
        public Guid? AutorId { get; set; }
        /// <summary>
        /// Obtém ou define a categoria da notícia.
        /// </summary>
        public string? Categoria { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
        /// <summary>
        /// Obtém ou define a data de publicação.
        /// </summary>
        public DateTime? DataPublicacao { get; set; }
        /// <summary>
        /// Obtém ou define as tags associadas à notícia.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
