using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um tipo de anúncio no sistema.
    /// </summary>
    [Table("tipos_anuncio")]
    public class TipoAnuncio
    {
        /// <summary>
        /// Obtém ou define o ID do tipo de anúncio.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do tipo de anúncio.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Total de registros (usado apenas para paginação, não mapeado no banco).
        /// </summary>
        [NotMapped]
        public int TotalCount { get; set; }
    }
}
