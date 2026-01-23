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
        string? titulo = null,
        string? descricao = null,
        string? conteudo = null)
    {
        return new RequestNoticia
        {
            Titulo = titulo ?? "Notícia de Teste Exclusiva",
            Descricao = descricao ?? "Descrição curta da notícia para listagem.",
            Conteudo = conteudo ?? "Conteúdo completo da notícia com detalhes importantes.",
            Categoria = "tecnologia", // minúsculo
            DataPublicacao = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria uma notícia com dados mínimos
    /// </summary>
    public static RequestNoticia CreateMinimalNoticia()
    {
        return new RequestNoticia
        {
            Titulo = "Notícia Mínima",
            Descricao = "Apenas descrição.",
            Conteudo = null, 
            Categoria = "geral" // minúsculo
        };
    }

    // ...

    /// <summary>
    /// Cria uma notícia para update
    /// </summary>
    public static RequestNoticia CreateUpdateNoticia(
        string? titulo = null)
    {
        return new RequestNoticia
        {
            Titulo = titulo ?? "Título da Notícia Atualizado",
            Descricao = "Descrição atualizada.",
            Conteudo = "Conteúdo atualizado.",
            Categoria = "academico", // minúsculo e existente
            DataAtualizacao = DateTime.UtcNow
        };
    }
}
