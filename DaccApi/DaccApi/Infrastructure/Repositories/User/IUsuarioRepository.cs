using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public void Add(RequestUsuario request);
      
        public List<Usuario> GetAll();

        public Task<Usuario?> GetUserById(Guid? id);

        public Task<Usuario?> GetUserByEmail(String email);
    }
}
