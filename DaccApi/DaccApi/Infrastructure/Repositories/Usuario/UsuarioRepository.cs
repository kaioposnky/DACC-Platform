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

        public async Task CreateUser(Usuario usuario)
        {
            var sql = _repositoryDapper.GetQueryNamed("InsertUsuario");
            var senhaHash = _argon2Utility.HashPassword(usuario.SenhaHash);
            var param = new
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = senhaHash,
                Telefone = usuario.Telefone,
                TipoUsuarioId = usuario.TipoUsuario
            };

            try
            {
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario?> GetUserById(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUsuarioById");

                var param = new { Id = id };

                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            } catch(Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Id na banco de dados!");
            }
            
        }

        public async Task<Usuario?> GetUserByEmail(string email)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUsuarioById");

                var param = new { Email = email };

                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            } catch(Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Email na banco de dados!");
            }

        }
    }
}
