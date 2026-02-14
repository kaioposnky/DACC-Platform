using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de projetos.
/// </summary>
public static class ProjetoTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestProjeto válido com dados padrão.
    /// </summary>
    public static RequestProjeto CreateValidProjeto(
        string? title = null,
        string? description = null,
        string? status = null,
        Guid? directorateId = null)
    {
        // IDs de diretorias válidos (veja sqlcode.sql INSERT INTO diretoria)
        // Como não temos UUIDs fixos, usaremos um valor padrão
        // que deve ser substituído por um ID real nos testes de integração
        var defaultDirectorateId = directorateId ?? Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        return new RequestProjeto
        {
            Title = title ?? "Projeto Inovador DACC",
            Description = description ?? "Um projeto focado em IA e automação para a comunidade.",
            Status = status ?? "planejado", // Valor válido do tipos_progresso
            DirectorateId = defaultDirectorateId, // GUID da diretoria
            Technologies = new[] { "IA", "Python", "DACC" },
            CompletionText = "Projeto finalizado com sucesso.",
            Progress = 0
        };
    }

    /// <summary>
    /// Cria um projeto com dados mínimos.
    /// </summary>
    public static RequestProjeto CreateMinimalProjeto()
    {
        return new RequestProjeto
        {
            Title = "Projeto Minimalista",
            Description = "Descrição essencial do projeto.",
            Status = "planejado",
            DirectorateId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Technologies = new[] { "Teste" },
            Progress = 0
        };
    }

    /// <summary>
    /// Cria um projeto para atualização.
    /// </summary>
    public static RequestProjeto CreateUpdateProjeto(string? title = null)
    {
        return new RequestProjeto
        {
            Title = title ?? "Projeto Atualizado",
            Description = "Nova descrição do projeto após atualização.",
            Status = "em progresso",
            DirectorateId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Technologies = new[] { "IA", "React" },
            CompletionText = "Quase lá!",
            Progress = 50
        };
    }
}
