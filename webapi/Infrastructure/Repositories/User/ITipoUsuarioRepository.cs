using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface ITipoUsuarioRepository
    {
        Task<List<TipoUsuario>> GetAllAsync();
    }
}
