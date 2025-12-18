using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public class AvaliacaoRepository : BaseRepository<AvaliacaoProduto>, IAvaliacaoRepository
{
    private readonly IRepositoryDapper _repositoryDapper;

    public AvaliacaoRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
    {
        _repositoryDapper = repositoryDapper;
    }
    
    /// <summary>
    /// Obtém todas as avaliações de um produto específico.
    /// </summary>
    public async Task<List<AvaliacaoProduto>> GetAvaliacoesByProductId(Guid produtoId)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAvaliacoesByProductId");
            var param = new { produtoId = produtoId };

            var queryResult = await _repositoryDapper.QueryAsync<AvaliacaoProduto>(sql, param);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter avaliação pelo Id do produto no banco de dados!"+ ex.Message);
        }
        
    }

    /// <summary>
    /// Obtém todas as avaliações de um usuário específico.
    /// </summary>
    public async Task<List<AvaliacaoProduto>> GetAvaliacoesByUserId(Guid usuarioId)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAvaliacoesByUserId");
            var param = new { usuarioId = usuarioId };
            
            var queryResult = await _repositoryDapper.QueryAsync<AvaliacaoProduto>(sql, param);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter avaliacao pelo Id do usuário no banco de dados!"+ ex.Message);
        }
    }
    
}