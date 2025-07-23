using System.Data;
using System.Security.Authentication;
using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using NHibernate.Exceptions;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.ValueConversion;

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
            var insertSql = _repositoryDapper.GetQueryNamed("InsertUsuario");
            var senhaHash = _argon2Utility.HashPassword(usuario.SenhaHash!);
            var param = new
            {
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Ra = usuario.Ra,
                Curso = usuario.Curso,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Senha = senhaHash,
                Cargo = usuario.Cargo,
            };

            try
            {
                var userIdResult = await _repositoryDapper.QueryAsync<int>(insertSql, param);
                var userId = userIdResult.SingleOrDefault();

                if (userId == 0) 
                {
                    throw new Exception("A inserção do usuário falhou ao retornar um ID.");
                }

            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    throw ex.ConstraintName switch
                    {
                        "usuario_email_key" => new InvalidConstraintException("Já existe um usuário com esse email!"),
                        "usuario_ra_key" => new InvalidConstraintException("Já existe um usuário com esse RA!"),
                        _ => new Exception()
                    };
                }
            } 
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com", ex);
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
                var sql = _repositoryDapper.GetQueryNamed("GetUsuarioByEmail");

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
