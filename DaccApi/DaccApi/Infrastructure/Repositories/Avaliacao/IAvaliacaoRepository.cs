
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public Task CreateAvaliacao(AvaliacaoProduto avaliacao);
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoes();
    
    public Task <AvaliacaoProduto?> GetAvaliacaoById(Guid id);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid produtoId);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(Guid usuarioId);
    public Task DeleteAvaliacao(Guid id);
    
    public Task UpdateAvaliacao(Guid id, RequestUpdateAvaliacao avaliacao);
}