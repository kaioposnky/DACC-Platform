using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;
        private readonly IArgon2Utility _argon2Utility;
        public UsuarioRepository(IRepositoryDapper repositoryDapper, 
            IArgon2Utility argon2Utility)
        {
            _repositoryDapper = repositoryDapper;
            _argon2Utility = argon2Utility;
        }

        public void Add(Usuario usuario)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("InsertUsuario");
                var encryptPassword = usuario.Password;

                var parameters = new
                {
                    Nome = usuario.Name,
                    Email = usuario.Email,
                    Senha = encryptPassword,
                    TypeId = (int)UserEnum.UserEnumTypeId.Master,
                    RegistrationDate = DateTime.Now,
                };


                _repositoryDapper.ExecuteAsync(sql, parameters).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o usuário na base de dados");
            }
        }

        public List<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
