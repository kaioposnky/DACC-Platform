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
        string? titulo = null,
        string? descricao = null,
        string? tipoEvento = null)
    {
        return new RequestEvento
        {
            Id = Guid.NewGuid(), // API provavelmente ignora no create, mas é bom ter
            Titulo = titulo ?? "Evento de Tecnologia DACC",
            Descricao = descricao ?? "Um evento incrível sobre as novidades do setor.",
            Data = DateTime.UtcNow.AddDays(7),
            TipoEvento = tipoEvento ?? "workshop", // Valor válido do banco
            TextoAcao = "Inscrever-se",
            LinkAcao = "https://dacc.com/evento/inscricao"
        };
    }

    /// <summary>
    /// Cria um evento com dados mínimos (assumindo que Data e Titulo sejam obrigatórios na validação do serviço)
    /// </summary>
    public static RequestEvento CreateMinimalEvento()
    {
        return new RequestEvento
        {
            Titulo = "Evento Mínimo",
            Descricao = "Descrição Obrigatória", // Validar se service exige
            Data = DateTime.UtcNow.AddDays(1),
            TipoEvento = "seminario",
            TextoAcao = "Ver",
            LinkAcao = "link"
        };
    }
    
    /// <summary>
    /// Cria um evento para update
    /// </summary>
    public static RequestEvento CreateUpdateEvento(
        string? titulo = null)
    {
        return new RequestEvento
        {
            Titulo = titulo ?? "Evento Atualizado",
            Descricao = "Nova descrição atualizada.",
            Data = DateTime.UtcNow.AddDays(14),
            TipoEvento = "hackathon",
            TextoAcao = "Participar",
            LinkAcao = "https://dacc.com/hackathon"
        };
    }
}
