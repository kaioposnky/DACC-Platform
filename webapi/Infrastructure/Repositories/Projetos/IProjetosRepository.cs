using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface IProjetosRepository 
    {
        Task<List<Projeto>> GetAllAsync();
        Task<Projeto?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Projeto entity);
        Task<bool> UpdateAsync(Guid id, Projeto entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
