using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de eventos
/// </summary>
public static class EventoTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestEvento válido com dados padrão
    /// </summary>
    public static RequestEvento CreateValidEvento(
        string? title = null,
        string? description = null,
        string? eventType = null)
    {
        return new RequestEvento
        {
            Id = Guid.NewGuid(), // API provavelmente ignora no create, mas é bom ter
            Title = title ?? "Evento de Tecnologia DACC",
            Description = description ?? "Um evento incrível sobre as novidades do setor.",
            Date = DateTime.UtcNow.AddDays(7),
            EventType = eventType ?? "workshop", // Valor válido do banco
            ActionText = "Inscrever-se",
            ActionLink = "https://dacc.com/evento/inscricao"
        };
    }

    /// <summary>
    /// Cria um evento com dados mínimos (assumindo que Data e Titulo sejam obrigatórios na validação do serviço)
    /// </summary>
    public static RequestEvento CreateMinimalEvento()
    {
        return new RequestEvento
        {
            Title = "Evento Mínimo",
            Description = "Descrição Obrigatória", // Validar se service exige
            Date = DateTime.UtcNow.AddDays(1),
            EventType = "seminario",
            ActionText = "Ver",
            ActionLink = "link"
        };
    }
    
    /// <summary>
    /// Cria um evento para update
    /// </summary>
    public static RequestEvento CreateUpdateEvento(
        string? title = null)
    {
        return new RequestEvento
        {
            Title = title ?? "Evento Atualizado",
            Description = "Nova descrição atualizada.",
            Date = DateTime.UtcNow.AddDays(14),
            EventType = "hackathon",
            ActionText = "Participar",
            ActionLink = "https://dacc.com/hackathon"
        };
    }
}
