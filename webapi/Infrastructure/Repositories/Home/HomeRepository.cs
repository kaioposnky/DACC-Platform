using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using DaccApi.Model.Objects.Noticia;
using Dapper;

namespace DaccApi.Infrastructure.Repositories.Home
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public HomeRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<(List<Noticia> Noticias, List<Evento> Eventos, List<Projeto> Projetos)> GetUnifiedFeedAsync(int limit)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUnifiedFeed");
                var param = new { Limit = limit };

                using (var connection = _repositoryDapper.GetConnection())
                {
                    using (var multi = await connection.QueryMultipleAsync(sql, param))
                    {
                        var noticias = (await multi.ReadAsync<Noticia>()).ToList();
                        var eventos = (await multi.ReadAsync<Evento>()).ToList();
                        var projetos = (await multi.ReadAsync<Projeto>()).ToList();

                        return (noticias, eventos, projetos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter feed unificado: {ex.Message}", ex);
            }
        }
    }
}
