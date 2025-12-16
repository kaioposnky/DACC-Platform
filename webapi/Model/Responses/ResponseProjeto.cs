namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de um projeto.
/// </summary>
public class ResponseProjeto
{
    /// <summary>
    /// Obtém ou define o ID do projeto.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o título do projeto.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Obtém ou define a descrição do projeto.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Obtém ou define o ícone do projeto. Mapeado de ImagemUrl do backend.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Obtém ou define as tecnologias associadas ao projeto. Mapeado de Tags do backend.
    /// </summary>
    public string[]? Technologies { get; set; }

    /// <summary>
    /// Obtém ou define o status do projeto.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Obtém ou define o progresso do projeto (frontend specific).
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// Obtém ou define o texto de conclusão do projeto (frontend specific).
    /// </summary>
    public string? CompletionText { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Projeto.
    /// </summary>
    /// <param name="projeto">A entidade Projeto de origem.</param>
    public ResponseProjeto(Projeto projeto)
    {
        Id = projeto.Id;
        Title = projeto.Titulo;
        Description = projeto.Descricao;
        Icon = projeto.ImagemUrl; // Mapeado de ImagemUrl
        Technologies = projeto.Tags; // Mapeado de Tags
        Status = projeto.Status;

        // Propriedades específicas do frontend sem correspondência direta
        Progress = 0; // Valor padrão
        CompletionText = string.Empty; // Valor padrão
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseProjeto()
    {
    }
}
