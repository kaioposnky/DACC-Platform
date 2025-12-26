using System.Text.Json;
using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using DaccApi.Model.Objects.Noticia;
using DaccApi.Model.Requests;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    public class NoticiasRepository : BaseRepository<Noticia>, INoticiasRepository
    {
        public NoticiasRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {

        }

        public async Task<List<Noticia>> SearchNoticias(RequestQueryNoticia queryNoticia)
        {
            var sql = _dapper.GetQueryNamed("SearchNoticias");

            var param = new
            {
                SearchQuery = string.IsNullOrEmpty(queryNoticia.SearchQuery) ? null : $"%{queryNoticia.SearchQuery}%",
                Limit = queryNoticia.Limit,
                Category = queryNoticia.Category,
                PublishDate = queryNoticia.PublishDate,
                Page = queryNoticia.Page
            };

            var result = await _dapper.QueryAsync<dynamic>(sql, param);
            return MapNoticias(result);
        }

        public new async Task<Noticia?> GetByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetNoticiaById");
            var param = new { id = id };

            var result = await _dapper.QueryAsync<dynamic>(sql, param);
            return MapNoticias(result).FirstOrDefault();
        }

        public async Task<List<Noticia>> GetAllNoticias()
        {
            var sql = _dapper.GetQueryNamed("GetAllNoticias");
            var result = await _dapper.QueryAsync<dynamic>(sql);
            return MapNoticias(result);
        }

        private List<Noticia> MapNoticias(IEnumerable<dynamic> rows)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return rows.Select(row =>
            {
                var noticia = new Noticia
                {
                    Id = row.id,
                    Titulo = row.titulo,
                    Descricao = row.descricao,
                    Conteudo = row.conteudo,
                    ImagemUrl = row.imagem_url,
                    ImagemAlt = row.imagem_alt,
                    AutorId = row.autor_id,
                    Categoria = row.categoria,
                    DataAtualizacao = row.data_atualizacao,
                    DataPublicacao = row.data_publicacao,
                    TempoLeitura = row.tempo_leitura
                };

                if (row.autorjson != null)
                {
                    string autorJson = row.autorjson;
                    noticia.Autor = JsonSerializer.Deserialize<NoticiaAutor>(autorJson, options)!;
                }

                if (row.tagsjson != null)
                {
                    string tagsJson = row.tagsjson;
                    noticia.Tags = JsonSerializer.Deserialize<List<NoticiaTag>>(tagsJson, options) ?? [];
                }

                return noticia;
            }).ToList();
        }
    }
}
