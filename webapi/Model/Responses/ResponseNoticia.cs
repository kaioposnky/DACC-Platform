namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de uma notícia.
/// </summary>
public class ResponseNoticia
{
    /// <summary>
    /// Obtém ou define o ID da notícia.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Obtém ou define o título da notícia.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Obtém ou define a descrição da notícia.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Obtém ou define o conteúdo da notícia.
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// Obtém ou define o autor da notícia (frontend specific).
    /// </summary>
    public string? Author { get; set; }
    /// <summary>
    /// Obtém ou define o tempo de leitura da notícia (frontend specific).
    /// </summary>
    public int? ReadTime { get; set; }
    /// <summary>
    /// Obtém ou define a URL da imagem da notícia.
    /// </summary>
    public string? Image { get; set; }
    /// <summary>
    /// Obtém ou define as tags da notícia (frontend specific).
    /// </summary>
    public string[]? Tags { get; set; }
    /// <summary>
    /// Obtém ou define a data de publicação da notícia.
    /// </summary>
    public DateTime? Date { get; set; }
    /// <summary>
    /// Obtém ou define a categoria da notícia.
    /// </summary>
    public string? Category { get; set; }
    /// <summary>
    /// Obtém ou define o ícone da notícia (frontend specific).
    /// </summary>
    public string? Icon { get; set; }
    /// <summary>
    /// Obtém ou define o gradiente da notícia (frontend specific).
    /// </summary>
    public string? Gradient { get; set; }
    /// <summary>
    /// Obtém ou define o link "Leia Mais" da notícia (frontend specific).
    /// </summary>
    public string? ReadMoreLink { get; set; }
    /// <summary>
    /// Obtém ou define a data da última atualização.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Noticia.
    /// </summary>
    /// <param name="noticia">A entidade Noticia de origem.</param>
    public ResponseNoticia(Noticia noticia)
    {
        Id = noticia.Id;
        Title = noticia.Titulo;
        Description = noticia.Descricao;
        Content = noticia.Conteudo;
        Image = noticia.ImagemUrl;
        Category = noticia.Categoria;
        Date = noticia.DataPublicacao;
        UpdatedAt = noticia.DataAtualizacao;
        Author = noticia.Autor.Nome + noticia.Autor.Sobrenome;
        ReadTime = noticia.TempoLeitura;
        Tags = noticia.Tags.Select(tag => tag.Nome).ToArray();
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseNoticia() { }
}

