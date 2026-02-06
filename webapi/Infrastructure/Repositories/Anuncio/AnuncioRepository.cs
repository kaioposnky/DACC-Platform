using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;

namespace DaccApi.Infrastructure.Repositories.Anuncio;

public class AnuncioRepository : BaseRepository<Model.Anuncio>, IAnuncioRepository
{
    public AnuncioRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
    {
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

        var result = (await _dapper.QueryAsync<Model.Anuncio>(sql, queryParams)).ToList();

        var totalCount = 0;
        if (result.Any())
        {
            totalCount = result.First().TotalCount;
        }

        return (result, totalCount);
    }
}