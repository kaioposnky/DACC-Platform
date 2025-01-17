using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        public void Add(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
