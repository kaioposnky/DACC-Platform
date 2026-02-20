using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio;

public class AnuncioRepository : BaseRepository<Model.Anuncio>, IAnuncioRepository
{
    public AnuncioRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
    {
    }
    public new async Task<List<Model.Anuncio>> GetAllAsync()
    {
        var sql = _dapper.GetQueryNamed("GetAllAnuncio");
        var result = await _dapper.QueryAsync<Model.Anuncio, Usuario, Model.Anuncio>(
            sql,
            (anuncio, usuario) =>
            {
                anuncio.Autor = usuario;
                MapDetails(anuncio);
                return anuncio;
            },
            splitOn: "Usuario_Id"
        );
        return result.ToList();
    }

    public new async Task<Model.Anuncio?> GetByIdAsync(Guid id)
    {
        var sql = _dapper.GetQueryNamed("GetAnuncioById");
        var result = await _dapper.QueryAsync<Model.Anuncio, Usuario, Model.Anuncio>(
            sql,
            (anuncio, usuario) =>
            {
                anuncio.Autor = usuario;
                MapDetails(anuncio);
                return anuncio;
            },
            new { id },
            splitOn: "Usuario_Id"
        );
        return result.FirstOrDefault();
    }

    public new async Task<bool> CreateAsync(Model.Anuncio entity)
    {
        var sql = _dapper.GetQueryNamed("CreateAnuncio");
        var id = await _dapper.QueryFirstAsync<Guid>(sql, entity);

        if (id != Guid.Empty && entity.Detalhes != null && entity.Detalhes.Any())
        {
            var sqlDetalhe = _dapper.GetQueryNamed("InsertAnuncioDetalhe");
            foreach (var (detalhe, index) in entity.Detalhes.Select((d, i) => (d, i)))
            {
                await _dapper.ExecuteAsync(sqlDetalhe, new
                {
                    AnuncioId = id,
                    Ordem = index,
                    ImagemUrl = detalhe.Icon,
                    Conteudo = detalhe.Text
                });
            }
        }
        return id != Guid.Empty;
    }

    public new async Task<bool> UpdateAsync(Guid id, Model.Anuncio entity)
    {
        var sql = _dapper.GetQueryNamed("UpdateAnuncio");
        var rows = await _dapper.ExecuteAsync(sql, new
        {
            id = id,
            Titulo = entity.Titulo,
            Conteudo = entity.Conteudo,
            TipoAnuncio = entity.TipoAnuncio,
            BotaoPrimarioTexto = entity.BotaoPrimarioTexto,
            BotaoPrimarioLink = entity.BotaoPrimarioLink,
            BotaoSecundarioTexto = entity.BotaoSecundarioTexto,
            BotaoSecundarioLink = entity.BotaoSecundarioLink,
            ImagemUrl = entity.ImagemUrl,
            ImagemAlt = entity.ImagemAlt,
            Icone = entity.Icone,
            Ativo = entity.Ativo,
            AutorId = entity.AutorId
        });

        if (rows > 0)
        {
            await _dapper.ExecuteAsync(_dapper.GetQueryNamed("DeleteAnuncioDetalhes"), new { AnuncioId = id });

            if (entity.Detalhes != null && entity.Detalhes.Any())
            {
                var sqlDetalhe = _dapper.GetQueryNamed("InsertAnuncioDetalhe");
                foreach (var (detalhe, index) in entity.Detalhes.Select((d, i) => (d, i)))
                {
                    await _dapper.ExecuteAsync(sqlDetalhe, new
                    {
                        AnuncioId = id,
                        Ordem = index,
                        ImagemUrl = detalhe.Icon,
                        Conteudo = detalhe.Text
                    });
                }
            }
        }
        return rows > 0;
    }

    public new async Task<bool> DeleteAsync(Guid id)
    {
        await _dapper.ExecuteAsync(_dapper.GetQueryNamed("DeleteAnuncioDetalhes"), new { AnuncioId = id });
        var sql = _dapper.GetQueryNamed("DeleteAnuncio");
        var rows = await _dapper.ExecuteAsync(sql, new { id = id });
        return rows > 0;
    }

    public async Task<(List<Model.Anuncio> Anuncios, int TotalCount)> SearchAnuncio(Model.Requests.RequestQueryAnuncio query)
    {
        var sql = _dapper.GetQueryNamed("SearchAnuncio");
        var queryParams = new
        {
            SearchPattern = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
            Page = query.Page,
            Limit = query.Limit,
            Type = query.Type,
            CreatedFrom = query.CreatedFrom.HasValue ? DateTime.SpecifyKind(query.CreatedFrom.Value, DateTimeKind.Utc) : (DateTime?)null,
            CreatedTo = query.CreatedTo.HasValue ? DateTime.SpecifyKind(query.CreatedTo.Value, DateTimeKind.Utc) : (DateTime?)null,
            IsActive = query.IsActive
        };

        var result = (await _dapper.QueryAsync<Model.Anuncio, Usuario, Model.Anuncio>(
            sql,
            (anuncio, usuario) =>
            {
                anuncio.Autor = usuario;
                MapDetails(anuncio);
                return anuncio;
            },
            queryParams,
            splitOn: "Usuario_Id"
        )).ToList();

        var totalCount = 0;
        if (result.Any())
        {
            totalCount = result.First().TotalCount;
        }

        return (result, totalCount);
    }

    private void MapDetails(Model.Anuncio anuncio)
    {
        if (!string.IsNullOrEmpty(anuncio.DetalhesJson))
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                anuncio.Detalhes = System.Text.Json.JsonSerializer.Deserialize<List<DaccApi.Model.Responses.DetailsItem>>(anuncio.DetalhesJson, options) ?? new List<DaccApi.Model.Responses.DetailsItem>();
            }
            catch
            {
                anuncio.Detalhes = new List<DaccApi.Model.Responses.DetailsItem>();
            }
        }
    }
}