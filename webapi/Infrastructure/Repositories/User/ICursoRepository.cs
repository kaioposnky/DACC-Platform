using DaccApi.Model.Objects;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface ICursoRepository
    {
        Task<List<Curso>> GetAllAsync();
        Task<Curso?> GetByIdAsync(Guid id);
        Task<Curso?> GetByNomeAsync(string nome);
        Task<Guid> CreateAsync(Curso curso);
        Task UpdateAsync(Guid id, Curso curso);
        Task DeleteAsync(Guid id);
    }
}
