namespace DaccApi.Model.Post;

/// <summary>
/// Representa um post no fórum.
/// </summary>
public class Post
{
    /// <summary>
    /// Obtém ou define o ID do post.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Obtém ou define o título do post.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Obtém ou define o conteúdo do post.
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// Obtém ou define as tags do post.
    /// </summary>
    public string Tags { get; set; }
}
