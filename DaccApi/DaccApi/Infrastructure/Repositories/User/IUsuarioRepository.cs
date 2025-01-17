using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        void Add(Usuario usuario);
        List<Usuario> GetAll();
    }
}
