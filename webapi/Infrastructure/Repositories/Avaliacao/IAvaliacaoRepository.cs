using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    Task<List<AvaliacaoProduto>> GetAllAsync();
    Task<AvaliacaoProduto?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(AvaliacaoProduto entity);
    Task<bool> UpdateAsync(Guid id, AvaliacaoProduto entity);
    Task<bool> DeleteAsync(Guid id);

    Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid produtoId);
    Task<AvaliacaoProduto?> GetByIdWithDetails(Guid id);
    Task<List<AvaliacaoProduto>> GetAllWithDetails();
    Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(Guid usuarioId);
    Task<(List<AvaliacaoProduto> Avaliacoes, int TotalCount)> SearchAvaliacoes(Model.Requests.RequestQueryAvaliacao query);
}