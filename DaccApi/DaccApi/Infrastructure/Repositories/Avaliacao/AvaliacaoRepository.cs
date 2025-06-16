using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Avaliacao;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly IRepositoryDapper _repositoryDapper;

    public AvaliacaoRepository(IRepositoryDapper repositoryDapper)
    {
        _repositoryDapper = repositoryDapper;
    }
    public async void CreateAvaliacaoAsync(AvaliacaoProduto avaliacaoProduto)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("CreateAvaliacao");

            await _repositoryDapper.ExecuteAsync(sql, avaliacaoProduto);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao adicionar avaliação do produto no banco de dados");
        }
    }
    
    public List<AvaliacaoProduto> GetAllAvaliacoes()
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllAvaliacoes");

            var queryResult = _repositoryDapper.Query<AvaliacaoProduto>(sql);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as avaliações no banco de dados!");
        }
    }

    public List<AvaliacaoProduto> GetAvaliacoesByProductId(Guid? id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAvalicoesProductId");
            var param = new { Id = id };

            var queryResult = _repositoryDapper.Query<AvaliacaoProduto>(sql, param);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter Produto pelo Id no banco de dados!");
        }
        
    }

    public List<AvaliacaoProduto> GetAvaliacoesByUserId(Guid? id)
    {
        var sql = _repositoryDapper.GetQueryNamed("GetProductAvaliacaoUserByUserId");
        var param = new { Id = id };
        
        
        var queryResult = _repositoryDapper.Query<AvaliacaoProduto>(sql, param);

        var avaliacoes = queryResult.ToList();
        return avaliacoes;
    }
}