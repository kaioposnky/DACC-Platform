namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de um evento.
/// </summary>
public class ResponseEvento
{
    /// <summary>
    /// Obtém ou define o ID do evento.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Obtém ou define o título do evento.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Obtém ou define a descrição do evento.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Obtém ou define a data do evento.
    /// </summary>
    public DateTime? Date { get; set; }
    /// <summary>
    /// Obtém ou define o horário do evento (frontend specific).
    /// </summary>
    public string? Time { get; set; }
    /// <summary>
    /// Obtém ou define o texto do botão de ação.
    /// </summary>
    public string? ActionText { get; set; }
    /// <summary>
    /// Obtém ou define o link da ação.
    /// </summary>
    public string? ActionLink { get; set; }
    /// <summary>
    /// Obtém ou define o tipo do evento.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Evento.
    /// </summary>
    /// <param name="evento">A entidade Evento de origem.</param>
    public ResponseEvento(Evento evento)
    {
        Id = evento.Id;
        Title = evento.Titulo;
        Description = evento.Descricao;
        Date = evento.Data;
        Type = evento.TipoEvento;
        ActionText = evento.TextoAcao;
        ActionLink = evento.LinkAcao;

        // Propriedades específicas do frontend sem correspondência direta
        Time = evento.Data?.ToString("HH:mm") ?? string.Empty; // Extrai apenas a hora e minuto
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseEvento() { }
}