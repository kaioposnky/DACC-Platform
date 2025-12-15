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
    public async Task CreateAvaliacao(AvaliacaoProduto avaliacao)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("CreateAvaliacao");

            await _repositoryDapper.ExecuteAsync(sql, avaliacao);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao adicionar avaliação do produto no banco de dados" + ex.Message);
        }
    }
    
    public async Task<List<AvaliacaoProduto>> GetAllAvaliacoes()
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllAvaliacoes");

            var queryResult = await _repositoryDapper.QueryAsync<AvaliacaoProduto>(sql);

            var avaliacoes = queryResult.ToList();
            return avaliacoes;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as avaliações no banco de dados!" + ex.Message);
        }
    }

    public async Task<AvaliacaoProduto?> GetAvaliacaoById(Guid id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAvaliacaoById");
            
            var param = new { id = id };

            var queryResult = await _repositoryDapper.QueryAsync<AvaliacaoProduto>(sql,param);

            var avaliacao = queryResult.FirstOrDefault();
            
            return avaliacao;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter avaliações no banco de dados!" + ex.Message);
        }
    }
    

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
    
    public async Task DeleteAvaliacao(Guid id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("DeleteAvaliacao");
            var param = new { id = id };
            await _repositoryDapper.ExecuteAsync(sql, param);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao deletar avalaicao." +  ex.Message);
        }
        
    }


    public async Task UpdateAvaliacao(Guid id, RequestUpdateAvaliacao avaliacao)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("UpdateAvaliacao");
            var param = new
            {
                id = id,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                   
            };
            await _repositoryDapper.ExecuteAsync(sql, param);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar avaliação." + ex.Message);
        };
        
    }
}