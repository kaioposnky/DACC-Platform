
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public void CreateAvaliacaoAsync(Model.AvaliacaoProduto avaliacaoProduto);
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoes();
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid? id);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(Guid? id);
}