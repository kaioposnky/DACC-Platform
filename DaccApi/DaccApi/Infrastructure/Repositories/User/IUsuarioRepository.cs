using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public void Add(RequestUsuario request);
        public List<Model.User> GetAll();
    }
}
