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
        string? titulo = null,
        string? descricao = null,
        string? status = null,
        string? diretoria = null)
    {
        return new RequestProjeto
        {
            Titulo = titulo ?? "Projeto Inovador DACC",
            Descricao = descricao ?? "Um projeto focado em IA e automação para a comunidade.",
            Status = status ?? "planejado", // Valor válido do tipos_progresso
            Diretoria = diretoria ?? "Inovação", // Atenção: deve existir na tabela diretoria
            Tags = new[] { "IA", "Python", "DACC" },
            TextoConclusao = "Projeto finalizado com sucesso."
        };
    }

    /// <summary>
    /// Cria um projeto com dados mínimos.
    /// </summary>
    public static RequestProjeto CreateMinimalProjeto()
    {
        return new RequestProjeto
        {
            Titulo = "Projeto Minimalista",
            Descricao = "Descrição essencial do projeto.",
            Status = "planejado",
            Diretoria = "Geral",
            Tags = new[] { "Teste" }
        };
    }

    /// <summary>
    /// Cria um projeto para atualização.
    /// </summary>
    public static RequestProjeto CreateUpdateProjeto(string? titulo = null)
    {
        return new RequestProjeto
        {
            Titulo = titulo ?? "Projeto Atualizado",
            Descricao = "Nova descrição do projeto após atualização.",
            Status = "em progresso",
            Diretoria = "Inovação",
            Tags = new[] { "IA", "React" },
            TextoConclusao = "Quase lá!"
        };
    }
}
