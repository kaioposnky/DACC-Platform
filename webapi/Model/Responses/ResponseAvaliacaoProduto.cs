namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de uma avaliação de produto, alinhada com a interface ProductReview do frontend.
/// </summary>
public class ResponseAvaliacaoProduto
{
    /// <summary>
    /// Obtém ou define o ID da avaliação.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Obtém ou define o ID do usuário que fez a avaliação.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Obtém ou define o nome do usuário.
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// Obtém ou define o avatar do usuário.
    /// </summary>
    public string? UserAvatar { get; set; }
    
    /// <summary>
    /// Obtém ou define a nota da avaliação.
    /// </summary>
    public double Rating { get; set; }
    
    /// <summary>
    /// Obtém ou define o título da avaliação.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Obtém ou define o comentário da avaliação.
    /// </summary>
    public string? Comment { get; set; }
    
    /// <summary>
    /// Obtém ou define a data em que a avaliação foi criada.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Obtém ou define a data da última atualização da avaliação.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade AvaliacaoProduto.
    /// </summary>
    /// <param name="avaliacaoProduto">A entidade AvaliacaoProduto de origem.</param>
    public ResponseAvaliacaoProduto(AvaliacaoProduto avaliacaoProduto)
    {
        Id = avaliacaoProduto.Id;
        UserId = avaliacaoProduto.UsuarioId;
        UserName = avaliacaoProduto.UsuarioNome;
        UserAvatar = avaliacaoProduto.UsuarioAvatar;
        Rating = avaliacaoProduto.Nota;
        Title = avaliacaoProduto.Titulo;
        Comment = avaliacaoProduto.Comentario;
        CreatedAt = avaliacaoProduto.DataPostada;
        UpdatedAt = avaliacaoProduto.DataAtualizacao;
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseAvaliacaoProduto() { }
}