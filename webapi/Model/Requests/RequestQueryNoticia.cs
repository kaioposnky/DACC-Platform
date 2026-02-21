namespace DaccApi.Model.Requests;

public class RequestQueryNoticia : BaseQueryRequest
{
    /// <summary>
    /// Obtém ou define a categoria para filtro.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Data de publicação da notícia.
    /// </summary>
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// Tags da notícia.
    /// </summary>
    public List<string>? Tags { get; set; }
}