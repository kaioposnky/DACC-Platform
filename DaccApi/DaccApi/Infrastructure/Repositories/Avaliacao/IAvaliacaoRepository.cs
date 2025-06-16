
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public void CreateAvaliacaoAsync(Model.AvaliacaoProduto avaliacaoProduto);
    public List<AvaliacaoProduto> GetAllAvaliacoes();
    public List<AvaliacaoProduto> GetAvaliacoesByProductId(Guid? id);
    public List<AvaliacaoProduto> GetAvaliacoesByUserId(Guid? id);
}