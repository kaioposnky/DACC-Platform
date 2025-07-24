using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    
    


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
            throw new Exception("Erro ao obter todas as notícias no banco de dados.");
        }
    }

    public async Task CreateNoticia(RequestNoticia noticia)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("CreateNoticia");
            
            var param = new
            {
                Titulo = noticia.Titulo,
                Descricao = noticia.Descricao,
                Conteudo = noticia.Conteudo,
            };

            await _repositoryDapper.ExecuteAsync(sql, param);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar notícia.");
        }
    }

    public async Task DeleteNoticia(int id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("DeleteNoticia");
            var param = new { id = id };
            await _repositoryDapper.ExecuteAsync(sql, param);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao deletar notícia.");
        }
    }
    
    public async Task<Noticia?> GetNoticiaById(int id)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("GetNoticiaById");
            
            var param = new { id = id };
            
            var queryResult = await _repositoryDapper.QueryAsync<Noticia>(sql,param);

            var noticias = queryResult.FirstOrDefault();
            
            return noticias;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter a notícia.");
        }
    }

    public async Task UpdateNoticia(int id, RequestNoticia noticia)
    {
        try
        {
            var sql = _repositoryDapper.GetQueryNamed("UpdateNoticia");
            var param = new
            {
                id = id,
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                Categoria = noticia.Categoria,
                Descricao = noticia.Descricao
            };
            await _repositoryDapper.ExecuteAsync(sql, param);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao obter a notícia especificada no banco de dados.");
        };
    }
 }
}