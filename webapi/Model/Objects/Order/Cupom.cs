using System.ComponentModel.DataAnnotations.Schema;
using DaccApi.Enum.Posts;

namespace DaccApi.Model.Objects.Order
{
    /// <summary>
    /// Representa um cupom de desconto no sistema.
    /// </summary>
    [Table("cupom")]
    public class Cupom
    {
        /// <summary>
        /// Identificador único do cupom.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Código único do cupom.
        /// </summary>
        [Column("codigo")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de desconto aplicado ao cupom.
        /// </summary>
        [Column("tipo_desconto")]
        public TipoDesconto TipoDesconto { get; set; } = TipoDesconto.Fixed;

        /// <summary>
        /// Valor do desconto aplicado ao cupom.
        /// </summary>
        [Column("valor")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data de expiração do cupom.
        /// </summary>
        [Column("data_expiracao")]
        public DateTime? DataExpiracao { get; set; }

        /// <summary>
        /// Limite de uso do cupom.
        /// </summary>
        [Column("limite_uso")]
        public int? LimiteUso { get; set; }

        /// <summary>
        /// Quantidade de vezes que o cupom já foi usado.
        /// </summary>
        [Column("uso_atual")]
        public int UsoAtual { get; set; } = 0;

        /// <summary>
        /// Indica se o cupom está ativo.
        /// </summary>
        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação do cupom.
        /// </summary>
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
