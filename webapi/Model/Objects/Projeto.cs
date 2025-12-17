using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um projeto no sistema.
    /// </summary>
    [Table("projeto")]
    public class Projeto
    {
        
        /// <summary>
        /// Obtém ou define o ID do projeto.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Obtém ou define o título do projeto.
        /// </summary>
        [Column("titulo")]
        public string Titulo { get; set; }
        
        /// <summary>
        /// Obtém ou define a descrição do projeto.
        /// </summary>
        [Column("descricao")]
        public string Descricao { get; set; }
        
        /// <summary>
        /// Obtém ou define a URL da imagem do projeto.
        /// </summary>
        [Column("imagem_url")]
        public string ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [NotMapped]
        public string? ImagemAlt { get; set; }
        
        /// <summary>
        /// Obtém ou define o status do projeto.
        /// </summary>
        [Column("status")]
        public string? Status { get; set; }
        
        /// <summary>
        /// Obtém ou define a diretoria responsável pelo projeto.
        /// </summary>
        [Column("diretoria")]
        public string? Diretoria { get; set; }
        
        /// <summary>
        /// Obtém ou define as tags associadas ao projeto.
        /// </summary>
        [Column("tags")]
        public string[]? Tags { get; set; }

        /// <summary>
        /// Obtém ou define o progresso do projeto.
        /// </summary>
        [Column("progresso")]
        public int Progresso { get; set; }

        /// <summary>
        /// Obtém ou define o texto de conclusão do projeto.
        /// </summary>
        [Column("texto_conclusao")]
        public string TextoConclusao { get; set; }

    }
}
