using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Model.Post;

/// <summary>
/// Representa um post no fórum.
/// </summary>
[Table("post")]
public class Post
{
    /// <summary>
    /// Obtém ou define o ID do post.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o título do post.
    /// </summary>
    [Column("titulo")]
    public string Titulo { get; set; }

    /// <summary>
    /// Obtém ou define o conteúdo do post.
    /// </summary>
    [Column("conteudo")]
    public string Conteudo { get; set; }

    /// <summary>
    /// Obtém ou define o ID do autor.
    /// </summary>
    [Column("autor_id")]
    public Guid? AutorId { get; set; }

    /// <summary>
    /// Obtém ou define as tags do post.
    /// </summary>
    [Column("tags")]
    public string[] Tags { get; set; }

    /// <summary>
    /// Indica se o post foi respondido.
    /// </summary>
    [Column("respondida")]
    public bool Respondida { get; set; }

    /// <summary>
    /// Quantidade de visualizações.
    /// </summary>
    [Column("visualizacoes")]
    public int Visualizacoes { get; set; }

    [Column("data_criacao")]
    public DateTime DataCriacao { get; set; }

    [Column("data_atualizacao")]
    public DateTime DataAtualizacao { get; set; }
}
