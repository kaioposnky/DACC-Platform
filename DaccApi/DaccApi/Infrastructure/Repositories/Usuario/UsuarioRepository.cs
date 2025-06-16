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

        public void CreateUser(RequestUsuario request)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("InsertUsuario");
                var encryptPassword = _argon2Utility.HashPassword(request.Senha);

                var parameters = new
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Senha = encryptPassword,
                    TipoUsuarioId = request.TipoUsuario,
                    DataCadastro = DateTime.UtcNow,
                    Situacao = (int)UserEnum.UserSituacao.Ativo,
                };

                _repositoryDapper.ExecuteAsync(sql, parameters).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o usuário na banco de dados");
            }
        }

        public List<Model.Usuario> GetAll()
        {
            throw new NotImplementedException();
        }

        public Usuario? GetUserById(Guid? id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUsuarioById");

                var param = new { Id = id };

                var queryResult = _repositoryDapper.Query<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            } catch(Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Id na banco de dados!");
            }
            
        }

        public Usuario? GetUserByEmail(string email)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUsuarioById");

                var param = new { Email = email };

                var queryResult = _repositoryDapper.Query<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            } catch(Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Email na banco de dados!");
            }

        }
    }
}
