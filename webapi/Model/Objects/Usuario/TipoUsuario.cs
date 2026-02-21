using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um tipo de usuário (cargo/role) no sistema.
    /// </summary>
    [Table("tipos_usuario")]
    public class TipoUsuario
    {
        /// <summary>
        /// Obtém ou define o ID do tipo de usuário.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do tipo de usuário.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }
    }
}
