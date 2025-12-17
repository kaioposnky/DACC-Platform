using DaccApi.Model;
using DaccApi.Model.Objects;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    public interface IDiretoresRepository
    {
        Task<List<Diretor>> GetAllAsync();
        Task<Diretor?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Diretor diretor);
        Task<bool> UpdateAsync(Guid id, Diretor diretor);
        Task<bool> DeleteAsync(Guid id);
    }
}