using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public void CreateUser(RequestUsuario request);
      
        public List<Usuario> GetAll();

        public Usuario? GetUserById(Guid? id);

        public Usuario? GetUserByEmail(string email);
    }
}
