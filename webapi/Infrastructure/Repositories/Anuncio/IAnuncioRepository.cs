namespace DaccApi.Infrastructure.Repositories.Anuncio
{
    public interface IAnuncioRepository
    {
        Task<List<Model.Anuncio>> GetAllAsync();
        Task<Model.Anuncio?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Model.Anuncio entity);
        Task<bool> UpdateAsync(Guid id, Model.Anuncio entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
