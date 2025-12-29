using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    public interface ICupomRepository
    {
        Task<Cupom?> GetByCodeAsync(string code);
        Task<Cupom?> GetByIdAsync(Guid id);
        Task<List<Cupom>> GetAllAsync();
        Task<bool> CreateAsync(Cupom entity);
        Task<bool> UpdateAsync(Guid id, Cupom entity);
        Task<bool> DeleteAsync(Guid id);
        Task IncrementUsageAsync(Guid id);
    }
}
