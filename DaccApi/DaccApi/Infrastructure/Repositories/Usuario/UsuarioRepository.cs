using System.Data;
using System.Security.Authentication;
using DaccApi.Enum.UserEnum;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using NHibernate.Exceptions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.ValueConversion;
using Npgsql;

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

        public async Task<List<Usuario>> GetAll()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllUsers");
                
                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql);
                
                var usuarios = queryResult.ToList();
                
                return usuarios;
            } 
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter lista de usuários!", ex);
            }
        }

        public async Task<Usuario?> GetUserById(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUserById");

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
                var sql = _repositoryDapper.GetQueryNamed("GetUserByEmail");

                var param = new { Email = email };

                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            } catch(Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Email na banco de dados!");
            }

        }

        public async Task<int> UpdateUser(Usuario user)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateUser");

                var param = new
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Sobrenome = user.Sobrenome,
                    Curso = user.Curso,
                    Telefone = user.Telefone,
                    ImagemUrl = user.ImagemUrl,
                    NewsletterSubscriber = user.NewsLetterSubscriber
                };

                return await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar informações do usuário!", ex);
            }
        }

        public async Task<int> DeleteUser(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteUser");

                var param = new { Id = id };

                return await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar usuário!", ex);
            }
        }
        
        public async Task<TokensUsuario> GetUserTokens(int id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUserTokens");
                
                var param = new { Id = id };
                
                var queryResult = await _repositoryDapper.QueryAsync<TokensUsuario>(sql, param);
                
                
                var tokensUsuario = queryResult.FirstOrDefault();
                
                return tokensUsuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter tokens do usuário!");
            }
        }

        public async Task UpdateUserTokens(int id, TokensUsuario tokensUsuario)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateUserTokens");

                var param = new
                {
                    Id = id,
                    AccessToken = tokensUsuario.AccessToken,
                    RefreshToken = tokensUsuario.RefreshToken
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Detail);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.MessageText);
                throw new Exception("Erro ao atualizar tokens do usuário!" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar tokens do usuário!" + ex.StackTrace);
            }
        }
    }
}
