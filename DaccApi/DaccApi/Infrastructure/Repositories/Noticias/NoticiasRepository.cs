using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

public class NoticiasRepository : INoticiasRepository
{
    private readonly IRepositoryDapper _repositoryDapper;

    public NoticiasRepository(IRepositoryDapper repositoryDapper)
    {
        _repositoryDapper = repositoryDapper;
    }

    public async Task<List<Noticia>> GetAllNoticias()
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllNoticias");

            var queryResult = await _repositoryDapper.QueryAsync<Noticia>(sql);

            var noticias = queryResult.ToList();
            return noticias;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter todas as noticias no banco de dados.");
        }
        
    } 
}