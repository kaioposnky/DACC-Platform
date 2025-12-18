using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um anúncio no sistema.
    /// </summary>
    [Table("anuncio")]
    public class Anuncio
    {
        /// <summary>
        /// Obtém ou define o ID do anúncio.
        /// </summary>
        [Column("id")]
        public Guid Id  { get; set; }

        /// <summary>
        /// Obtém ou define o título do anúncio.
        /// </summary>
        [Column("titulo")]
        public string Titulo  { get; set; }

        /// <summary>
        /// Obtém ou define o conteúdo do anúncio.
        /// </summary>
        [Column("conteudo")]
        public string Conteudo { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do anúncio.
        /// </summary>
        [Column("tipo_anuncio")]
        public string TipoAnuncio { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão primário.
        /// </summary>
        [Column("botao_primario_texto")]
        public string BotaoPrimarioTexto { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão primário.
        /// </summary>
        [Column("botao_primario_link")]
        public string BotaoPrimarioLink { get; set; }

        /// <summary>
        /// Obtém ou define o texto do botão secundário.
        /// </summary>
        [Column("botao_secundario_texto")]
        public string BotaoSecundarioTexto { get; set; }

        /// <summary>
        /// Obtém ou define o link do botão secundário.
        /// </summary>
        [Column("botao_secundario_link")]
        public string BotaoSecundarioLink { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem do anúncio.
        /// </summary>
        [Column("imagem_url")]
        public string ImagemUrl { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [Column("imagem_alt")]
        public string ImagemAlt { get; set; }

        /// <summary>
        /// Obtém ou define se o anúncio está ativo.
        /// </summary>
        [Column("ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// Obtém ou define o ID do autor do anúncio.
        /// </summary>
        [Column("autor_id")]
        public Guid? AutorId { get; set; }

        /// <summary>
        /// Obtém ou define a data de criação do anúncio.
        /// </summary>
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Obtém ou define a data de atualização do anúncio.
        /// </summary>
        [Column("data_atualizacao")]
        public DateTime DataAtualizacao { get; set; }
    }
}