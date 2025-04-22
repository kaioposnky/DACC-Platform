
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public void CreateAvaliacaoAsync(Model.AvaliacaoProduto avaliacaoProduto);
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoesAsync();
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByProductIdAsync(Guid? id);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByUserIdAsync(Guid? id);
}