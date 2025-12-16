using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Noticias;

/// <summary>
/// Define a interface para o repositório de notícias.
/// </summary>
public interface INoticiasRepository
{
    /// <summary>
    /// Obtém todas as notícias.
    /// </summary>
    public Task<List<Noticia>> GetAllNoticias();
    /// <summary>
    /// Cria uma nova notícia.
    /// </summary>
    public Task CreateNoticia(Noticia noticia);
    /// <summary>
    /// Deleta uma notícia existente.
    /// </summary>
    public Task DeleteNoticia(Guid id);
    /// <summary>
    /// Obtém uma notícia específica pelo seu ID.
    /// </summary>
    public Task<Noticia?> GetNoticiaById(Guid id);
    /// <summary>
    /// Atualiza uma notícia existente.
    /// </summary>
    public Task UpdateNoticia(Guid id, Noticia noticia);
}