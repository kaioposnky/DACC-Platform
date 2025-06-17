using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public void CreateUser(RequestUsuario request);
      
        public List<Usuario> GetAll();

        public Task<Usuario?> GetUserById(Guid? id);

        public Task<Usuario?> GetUserByEmail(string email);
    }
}
