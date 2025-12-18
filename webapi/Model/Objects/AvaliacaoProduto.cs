using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a avaliação de um produto feita por um usuário.
    /// </summary>
    [Table("avaliacao")]
    public class AvaliacaoProduto
    {
        /// <summary>
        /// Obtém ou define o ID da avaliação.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define a nota da avaliação (ex: de 1 a 5).
        /// </summary>
        [Column("nota")]
        public double Nota { get; set; }

        /// <summary>
        /// Obtém ou define o ID do usuário que fez a avaliação.
        /// </summary>
        [Column("usuario_id")]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Obtém ou define o comentário da avaliação.
        /// </summary>
        [Column("comentario")]
        public string? Comentario { get; set; }

        /// <summary>
        /// Obtém ou define o ID do produto avaliado.
        /// </summary>
        [Column("produto_id")]
        public Guid ProdutoId { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// Obtém ou define a data em que a avaliação foi postada.
        /// </summary>
        [Column("data_avaliacao")]
        public DateTime DataPostada { get; set; }

        [Column("data_atualizacao")]
        public DateTime DataAtualizacao { get; set; }
    }
}
