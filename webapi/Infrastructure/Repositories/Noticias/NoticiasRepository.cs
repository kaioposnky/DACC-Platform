using System.Text.Json;
using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using DaccApi.Model.Objects.Noticia;
using DaccApi.Model.Requests;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    public class NoticiasRepository : BaseRepository<Noticia>, INoticiasRepository
    {
        public NoticiasRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
        }

        public async Task<(List<Noticia> Noticias, int TotalCount)> SearchNoticias(RequestQueryNoticia query)
        {
            var sql = _dapper.GetQueryNamed("SearchNoticias");
            var queryParams = new
            {
                SearchQuery = string.IsNullOrEmpty(query.SearchQuery) ? null : $"%{query.SearchQuery}%",
                Category = query.Category,
                PublishDate = query.PublishDate,
                Offset = (query.Page - 1) * query.Limit,
                Limit = query.Limit
            };

            var result = (await _dapper.QueryAsync<Noticia, Usuario, Noticia>(
                sql,
                (noticia, usuario) =>
                {
                    noticia.Autor = usuario;
                    MapTags(noticia);
                    return noticia;
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

        public new async Task<Noticia?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetNoticiaById");
            var param = new { id = id };

            var result = await _dapper.QueryAsync<Noticia, Usuario, Noticia>(
                sql,
                (noticia, usuario) =>
                {
                    noticia.Autor = usuario;
                    MapTags(noticia);
                    return noticia;
                },
                param,
                splitOn: "Usuario_Id"
            );

            return result.FirstOrDefault();
        }

        private void MapTags(Noticia noticia)
        {
            if (!string.IsNullOrEmpty(noticia.TagsJson))
            {
                try
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    noticia.Tags = JsonSerializer.Deserialize<List<NoticiaTag>>(noticia.TagsJson, options) ?? [];
                }
                catch
                {
                    noticia.Tags = [];
                }
            }
        }
    }
}
