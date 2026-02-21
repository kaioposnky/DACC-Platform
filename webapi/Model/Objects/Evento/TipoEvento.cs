using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um tipo de evento no sistema.
    /// </summary>
    [Table("tipos_evento")]
    public class TipoEvento
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }
    }
}
