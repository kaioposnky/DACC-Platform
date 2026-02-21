using DaccApi.Model.Objects;
using DaccApi.Model.Requests.Projetos;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface IDiretoriaRepository
    {
        Task<List<Diretoria>> GetAllAsync();
        Task<Diretoria?> GetByIdAsync(Guid id);
        Task<(List<Diretoria> Directorates, int TotalCount)> SearchAsync(RequestQueryDiretoria query);
        Task<bool> CreateAsync(Diretoria diretoria);
        Task<bool> UpdateAsync(Guid id, Diretoria diretoria);
        Task<bool> DeleteAsync(Guid id);
    }
}
