using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model.Objects
{
    /// <summary>
    /// Representa uma diretoria do DACC.
    /// </summary>
    [Table("diretoria")]
    public class Diretoria
    {
        /// <summary>
        /// Obtém ou define o ID da diretoria.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome da diretoria.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da diretoria.
        /// </summary>
        [Column("descricao")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Total de registros (usado apenas para paginação, não mapeado no banco).
        /// </summary>
        [NotMapped]
        public int TotalCount { get; set; }
    }
}
