using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um tipo de progresso de projeto no sistema.
    /// </summary>
    [Table("tipos_progresso")]
    public class TipoProgresso
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }
    }
}
