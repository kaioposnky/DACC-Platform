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
        string? department = null)
    {
        return new RequestProjeto
        {
            Title = title ?? "Projeto Inovador DACC",
            Description = description ?? "Um projeto focado em IA e automação para a comunidade.",
            Status = status ?? "planejado", // Valor válido do tipos_progresso
            Department = department ?? "Inovação", // Atenção: deve existir na tabela diretoria
            Tags = new[] { "IA", "Python", "DACC" },
            CompletionText = "Projeto finalizado com sucesso."
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
            Department = "Geral",
            Tags = new[] { "Teste" }
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
            Department = "Inovação",
            Tags = new[] { "IA", "React" },
            CompletionText = "Quase lá!"
        };
    }
}
