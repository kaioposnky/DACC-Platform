using DaccApi.Model;
using DaccApi.Model.Objects;
using DaccApi.Model.Requests;

namespace DaccApi.Infrastructure.Repositories.Professores
{
    public interface IProfessoresRepository
    {
        Task<List<Professor>> GetAllAsync();
        Task<Professor?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Professor professor);
        Task<bool> UpdateAsync(Guid id, Professor professor);
        Task<bool> DeleteAsync(Guid id);
        Task<(List<Professor> Professores, int TotalCount)> SearchProfessores(RequestQueryProfessor query);
    }
}
