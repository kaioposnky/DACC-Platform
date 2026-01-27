using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de notícias
/// </summary>
public static class NoticiaTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestNoticia válido com dados padrão
    /// </summary>
    public static RequestNoticia CreateValidNoticia(
        string? title = null,
        string? description = null,
        string? content = null)
    {
        return new RequestNoticia
        {
            Title = title ?? "Notícia de Teste Exclusiva",
            Description = description ?? "Descrição curta da notícia para listagem.",
            Content = content ?? "Conteúdo completo da notícia com detalhes importantes.",
            Category = "tecnologia", // minúsculo
            PublishedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria uma notícia com dados mínimos
    /// </summary>
    public static RequestNoticia CreateMinimalNoticia()
    {
        return new RequestNoticia
        {
            Title = "Notícia Mínima",
            Description = "Apenas descrição.",
            Content = null, 
            Category = "geral" // minúsculo
        };
    }

    // ...

    /// <summary>
    /// Cria uma notícia para update
    /// </summary>
    public static RequestNoticia CreateUpdateNoticia(
        string? title = null)
    {
        return new RequestNoticia
        {
            Title = title ?? "Título da Notícia Atualizado",
            Description = "Descrição atualizada.",
            Content = "Conteúdo atualizado.",
            Category = "academico", // minúsculo e existente
            UpdatedAt = DateTime.UtcNow
        };
    }
}
