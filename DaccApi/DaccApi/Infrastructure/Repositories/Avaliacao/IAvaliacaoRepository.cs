
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public void AddProductRatingAsync(Model.AvaliacaoProduto avaliacaoProduto);
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoesAsync();
}