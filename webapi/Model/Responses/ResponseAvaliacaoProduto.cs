namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de uma avaliação de produto, adaptada para o frontend ProductReview.
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
    /// Obtém ou define o nome do usuário (frontend specific, a ser preenchido por serviço).
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// Obtém ou define o avatar do usuário (frontend specific, a ser preenchido por serviço).
    /// </summary>
    public string? UserAvatar { get; set; }
    /// <summary>
    /// Obtém ou define a nota da avaliação.
    /// </summary>
    public double Rating { get; set; }
    /// <summary>
    /// Obtém ou define o título da avaliação (frontend specific).
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Obtém ou define o comentário da avaliação.
    /// </summary>
    public string? Comment { get; set; }
    /// <summary>
    /// Obtém ou define a data em que a avaliação foi postada.
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Obtém ou define se a avaliação foi verificada (frontend specific).
    /// </summary>
    public bool Verified { get; set; }
    /// <summary>
    /// Obtém ou define a contagem de utilidade da avaliação (frontend specific).
    /// </summary>
    public int Helpful { get; set; }
    /// <summary>
    /// Obtém ou define o ID do produto avaliado.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade AvaliacaoProduto.
    /// </summary>
    /// <param name="avaliacaoProduto">A entidade AvaliacaoProduto de origem.</param>
    public ResponseAvaliacaoProduto(AvaliacaoProduto avaliacaoProduto)
    {
        Id = avaliacaoProduto.Id;
        UserId = avaliacaoProduto.UsuarioId;
        Rating = avaliacaoProduto.Nota;
        Comment = avaliacaoProduto.Comentario;
        Date = avaliacaoProduto.DataPostada;
        ProductId = avaliacaoProduto.ProdutoId;

        // Propriedades específicas do frontend sem correspondência direta em AvaliacaoProduto
        // Devem ser preenchidas por um serviço que consulta detalhes do usuário
        UserName = null;
        UserAvatar = null;
        Title = null;
        Verified = false; // Valor padrão
        Helpful = 0; // Valor padrão
    }


    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseAvaliacaoProduto() { }
}