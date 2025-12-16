namespace DaccApi.Model
{
    /// <summary>
    /// Representa um anúncio no sistema.
    /// </summary>
    public class Anuncio
    {
        /// <summary>
        /// Obtém ou define o ID do anúncio.
        /// </summary>
        public Guid Id  { get; set; }
        /// <summary>
        /// Obtém ou define o título do anúncio.
        /// </summary>
        public string? Titulo  { get; set; }
        /// <summary>
        /// Obtém ou define o conteúdo do anúncio.
        /// </summary>
        public string? Conteudo { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do anúncio.
        /// </summary>
        public string? TipoAnuncio { get; set; }
        /// <summary>
        /// Obtém ou define a URL da imagem do anúncio.
        /// </summary>
        public string? ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImagemAlt { get; set; }
        /// <summary>
        /// Obtém ou define se o anúncio está ativo.
        /// </summary>
        public bool Ativo { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor do anúncio.
        /// </summary>
        public Guid AutorId { get; set; }
        /// <summary>
        /// Obtém ou define a data de criação do anúncio.
        /// </summary>
        public DateTime? DataCriacao { get; set; }
    }
}