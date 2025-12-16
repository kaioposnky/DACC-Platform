
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

/// <summary>
/// Define a interface para o repositório de avaliações de produtos.
/// </summary>
public interface IAvaliacaoRepository
{
    /// <summary>
    /// Cria uma nova avaliação de produto.
    /// </summary>
    public Task CreateAvaliacao(AvaliacaoProduto avaliacao);
    /// <summary>
    /// Obtém todas as avaliações.
    /// </summary>
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoes();
    
    /// <summary>
    /// Obtém uma avaliação específica pelo seu ID.
    /// </summary>
    public Task <AvaliacaoProduto?> GetAvaliacaoById(Guid id);
    /// <summary>
    /// Obtém todas as avaliações de um produto específico.
    /// </summary>
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid produtoId);
    /// <summary>
    /// Obtém todas as avaliações de um usuário específico.
    /// </summary>
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(Guid usuarioId);
    /// <summary>
    /// Deleta uma avaliação existente.
    /// </summary>
    public Task DeleteAvaliacao(Guid id);
    
    /// <summary>
    /// Atualiza uma avaliação existente.
    /// </summary>
    public Task UpdateAvaliacao(Guid id, RequestUpdateAvaliacao avaliacao);
}