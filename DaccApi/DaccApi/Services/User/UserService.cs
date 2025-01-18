using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;

namespace DaccApi.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UserService(IUsuarioRepository usuarioRepository) 
        {
            usuarioRepository = _usuarioRepository;
        }

        public bool Add(Usuario usuario) 
        {
            try
            {
                _usuarioRepository.Add(usuario);

                return true;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.dacc@gmail.com ");
            }
        }
    }
}
