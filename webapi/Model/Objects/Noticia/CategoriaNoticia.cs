using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma categoria de notícia no sistema.
    /// </summary>
    [Table("categorias_noticia")]
    public class CategoriaNoticia
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }
    }
}
