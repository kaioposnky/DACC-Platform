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

        public void Add(RequestUsuario request)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("InsertUsuario");
                var encryptPassword = _argon2Utility.HashPassword(request.Password);

                var parameters = new
                {
                    Nome = request.Name,
                    Email = request.Email,
                    Senha = encryptPassword,
                    TipoUsuarioId = request.TypeId,
                    DataCadastro = DateTime.UtcNow,
                    Situacao = (int)UserEnum.UserSituacao.Ativo,
                };


                _repositoryDapper.ExecuteAsync(sql, parameters).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o usuário na base de dados");
            }
        }

        public List<Model.User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
