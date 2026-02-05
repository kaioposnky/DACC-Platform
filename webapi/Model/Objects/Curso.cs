using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model.Objects
{
    /// <summary>
    /// Representa um curso no sistema.
    /// </summary>
    [Table("curso")]
    public class Curso
    {
        /// <summary>
        /// Obtém ou define o ID do curso.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do curso.
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
