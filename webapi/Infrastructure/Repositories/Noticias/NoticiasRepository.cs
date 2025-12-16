using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias
{
    /// <summary>
    /// Implementação do repositório de notícias.
    /// </summary>
    public class NoticiasRepository : INoticiasRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NoticiasRepository"/>.
        /// </summary>
        public NoticiasRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        /// <summary>
        /// Obtém todas as notícias.
        /// </summary>
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
                throw new Exception("Erro ao obter todas as notícias no banco de dados." + ex.Message);
            }
        }

        /// <summary>
        /// Cria uma nova notícia.
        /// </summary>
        public async Task CreateNoticia(Noticia noticia)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateNoticia");
                
                var param = new
                {
                    Titulo = noticia.Titulo,
                    Descricao = noticia.Descricao,
                    Conteudo = noticia.Conteudo,
                    AutorId = noticia.AutorId
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar notícia." + ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma notícia existente.
        /// </summary>
        public async Task DeleteNoticia(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteNoticia");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar notícia." + ex.Message);
            }
        }
        
        /// <summary>
        /// Obtém uma notícia específica pelo seu ID.
        /// </summary>
        public async Task<Noticia?> GetNoticiaById(Guid id)
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
                throw new Exception("Erro ao obter a notícia." + ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma notícia existente.
        /// </summary>
        public async Task UpdateNoticia(Guid id, Noticia noticia)
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
                    Descricao = noticia.Descricao,
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter a notícia especificada no banco de dados." + ex.Message);
            };
        }
    }
}