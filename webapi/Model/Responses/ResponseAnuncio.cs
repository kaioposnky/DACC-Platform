namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de um anúncio.
/// </summary>
public class ResponseAnuncio
{
    /// <summary>
    /// Obtém ou define o ID do anúncio.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Obtém ou define o título do anúncio.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Obtém ou define o conteúdo do anúncio.
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// Obtém ou define o tipo do anúncio.
    /// </summary>
    public string? Type { get; set; }
    /// <summary>
    /// Obtém ou define o ícone do anúncio.
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// Obtém ou define os detalhes adicionais do anúncio.
    /// </summary>
    public List<DetailsItem> Details { get; set; }
    /// <summary>
    /// Obtém ou define o texto do botão primário de ação.
    /// </summary>
    public string PrimaryButtonText { get; set; }
    /// <summary>
    /// Obtém ou define o texto do botão secundário de ação.
    /// </summary>
    public string SecondaryButtonText { get; set; }
    /// <summary>
    /// Obtém ou define o link do botão primário de ação.
    /// </summary>
    public string PrimaryButtonLink { get; set; }
    /// <summary>
    /// Obtém ou define o link do botão secundário de ação.
    /// </summary>
    public string SecondaryButtonLink { get; set; }
    /// <summary>
    /// Obtém ou define a URL da imagem do anúncio.
    /// </summary>
    public string? ImageSrc { get; set; }
    /// <summary>
    /// Obtém ou define o texto alternativo da imagem.
    /// </summary>
    public string? ImageAlt { get; set; }
    /// <summary>
    /// Obtém ou define a data de criação do anúncio.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Anuncio.
    /// </summary>
    /// <param name="anuncio">A entidade Anuncio de origem.</param>
    public ResponseAnuncio(Anuncio anuncio)
    {
        Id = anuncio.Id;
        Title = anuncio.Titulo;
        Content = anuncio.Conteudo;
        Type = anuncio.TipoAnuncio;
        ImageSrc = anuncio.ImagemUrl;
        ImageAlt = anuncio.ImagemAlt;
        CreatedAt = anuncio.DataCriacao;

        // Propriedades específicas do frontend sem correspondência direta na entidade Anuncio
        Icon = string.Empty; // Valor padrão
        Details = new List<DetailsItem>(); // Lista vazia padrão
        PrimaryButtonText = string.Empty;
        SecondaryButtonText = string.Empty;
        PrimaryButtonLink = string.Empty;
        SecondaryButtonLink = string.Empty;
    }
}

/// <summary>
/// Representa um item de detalhe para um anúncio (para o frontend).
/// </summary>
public class DetailsItem
{
    /// <summary>
    /// Obtém ou define o ícone do detalhe.
    /// </summary>
    public required string Icon { get; set; }
    /// <summary>
    /// Obtém ou define o texto do detalhe.
    /// </summary>
    public required string Text { get; set; }
}
