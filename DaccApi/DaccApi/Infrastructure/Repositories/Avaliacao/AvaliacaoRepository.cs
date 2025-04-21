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
    public async void AddProductRatingAsync(Model.AvaliacaoProduto avaliacaoProduto)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("");

            await _repositoryDapper.ExecuteAsync(sql, avaliacaoProduto);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao adicionar avaliação do produto no banco de dados");
        }
    }
    
    public async Task<List<AvaliacaoProduto>> GetAllAvaliacoesAsync()
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllAvaliacoes");

            var queryResult = await _repositoryDapper.QueryProcedureAsync<AvaliacaoProduto>(sql);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as avaliações no banco de dados!");
        }
    }
}