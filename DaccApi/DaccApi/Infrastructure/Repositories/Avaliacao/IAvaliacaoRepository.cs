
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public interface IAvaliacaoRepository
{
    public Task CreateAvaliacao(RequestCreateAvaliacao avaliacao);
    public Task<List<AvaliacaoProduto>> GetAllAvaliacoes();
    
    public Task <AvaliacaoProduto?> GetAvaliacaoById(int id);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid produtoId);
    public Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(int usuarioId);
    public Task DeleteAvaliacao(int id);
    
    public Task UpdateAvaliacao(int id, RequestUpdateAvaliacao avaliacao);
}