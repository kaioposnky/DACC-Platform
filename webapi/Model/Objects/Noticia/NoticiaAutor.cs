namespace DaccApi.Model.Objects.Noticia;

public class NoticiaAutor
{
    /// <summary>
    /// Obtém ou define o ID do autor.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome do autor.
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// Obtém ou define o sobrenome do autor.
    /// </summary>
    public string Sobrenome { get; set; }
}