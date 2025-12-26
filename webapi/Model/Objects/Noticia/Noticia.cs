using System.ComponentModel.DataAnnotations.Schema;
using DaccApi.Model.Objects.Noticia;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma notícia no sistema.
    /// </summary>
    [Table("noticia")]
    public class Noticia
    {
        /// <summary>
        /// Obtém ou define o ID da notícia.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o título da notícia.
        /// </summary>
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define a descrição da notícia.
        /// </summary>
        [Column("descricao")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define o conteúdo da notícia.
        /// </summary>
        [Column("conteudo")]
        public string? Conteudo { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem da notícia.
        /// </summary>
        [Column("imagem_url")]
        public string? ImagemUrl { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [Column("imagem_alt")]
        public string? ImagemAlt { get; set; }

        /// <summary>
        /// Obtém ou define o ID do autor da notícia.
        /// </summary>
        [Column("autor_id")]
        public Guid? AutorId { get; set; }

        /// <summary>
        /// Obtém ou define a categoria da notícia.
        /// </summary>
        [Column("categoria")]
        public string? Categoria { get; set; }

        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        [Column("data_atualizacao")]
        public DateTime? DataAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define a data de publicação.
        /// </summary>
        [Column("data_publicacao")]
        public DateTime? DataPublicacao { get; set; }

        /// <summary>
        /// Obtém ou define a data de publicação.
        /// </summary>
        [Column("tempo_leitura")]
        public int? TempoLeitura { get; set; }

        /// <summary>
        /// Obtém ou define as tags associadas à notícia.
        /// </summary>
        [NotMapped]
        public List<NoticiaTag> Tags { get; set; } = [];

        /// <summary>
        /// Autor da notícia.
        /// </summary>
        [NotMapped]
        public NoticiaAutor? Autor { get; set; }
    }
}
