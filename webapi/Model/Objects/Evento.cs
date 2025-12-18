using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um evento no sistema.
    /// </summary>
    [Table("evento")]
    public class Evento

    {
        /// <summary>
        /// Obtém ou define o ID do evento.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o título do evento.
        /// </summary>
        [Column("titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do evento.
        /// </summary>
        [Column("descricao")]
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define a data do evento.
        /// </summary>
        [Column("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do evento.
        /// </summary>
        [Column("tipo_evento")]
        public string? TipoEvento { get; set; }

        /// <summary>
        /// Obtém ou define o ID do autor do evento.
        /// </summary>
        [Column("autor_id")]
        public Guid? AutorId { get; set; }

        /// <summary>
        /// Obtém ou define o texto para o botão de ação.
        /// </summary>
        [Column("texto_acao")]
        public string TextoAcao { get; set; }

        /// <summary>
        /// Obtém ou define o link para a ação.
        /// </summary>
        [Column("link_acao")]
        public string LinkAcao { get; set; }
    
    }
}