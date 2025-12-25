using System.Data;
using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Npgsql;

namespace DaccApi.Infrastructure.Repositories.User
{
public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public UsuarioRepository(IRepositoryDapper repositoryDapper) : base(repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        public async Task<Guid> CreateUser(Usuario usuario)
        {
            var insertSql = _repositoryDapper.GetQueryNamed("InsertUsuario");
            var param = new
            {
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Ra = usuario.Ra,
                Curso = usuario.Curso,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Senha = usuario.SenhaHash,
                Cargo = usuario.Cargo,
                ImagemUrl = usuario.ImagemUrl,
            };

            try
            {
                var userIdResult = await _repositoryDapper.QueryAsync<Guid>(insertSql, param);
                var userId = userIdResult.SingleOrDefault();

                return userId;
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    throw ex.ConstraintName switch
                    {
                        "usuario_email_key" => new InvalidConstraintException(
                            "Já existe um usuário com esse email!"),
                        "usuario_ra_key" => new InvalidConstraintException("Já existe um usuário com esse RA!"),
                        _ => new Exception()
                    };
                }

                throw new Exception(
                    "Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com",
                    ex);
            }
        }

        /// <summary>
        /// Obtém um usuário específico pelo seu e-mail.
        /// </summary>
        public async Task<Usuario?> GetUserByEmail(string email)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUserByEmail");

                var param = new { Email = email };

                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo Email na banco de dados!" + ex.Message);
            }

        }

        /// <summary>
        /// Obtém um usuário específico pelo seu RA.
        /// </summary>
        public async Task<Usuario?> GetUserByRa(string ra)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUserByRA");

                var param = new { Ra = ra };

                var queryResult = await _repositoryDapper.QueryAsync<Usuario>(sql, param);

                var usuario = queryResult.FirstOrDefault();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter usuário pelo RA na banco de dados!" + ex.Message);
            }

        }


        /// <summary>
        /// Obtém os tokens de um usuário.
        /// </summary>
        public async Task<TokensUsuario> GetUserTokens(Guid id)
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
                throw new Exception("Erro ao obter tokens do usuário!" + ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os tokens de um usuário.
        /// </summary>
        public async Task UpdateUserTokens(Guid id, TokensUsuario tokensUsuario)
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
                throw new Exception("Erro ao atualizar tokens do usuário!" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar tokens do usuário!" + ex.Message);
            }
        }

        public async Task<EstatisticasUsuario?> GetUserStats(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetUserStats");
                var param = new { Id = id };

                var queryResult = await _repositoryDapper.QueryAsync<EstatisticasUsuario>(sql, param);

                return queryResult.FirstOrDefault();
            } catch (Exception ex)
            {
                throw new Exception("Erro ao obter estatísticas do usuário!" + ex.Message);
            }
        }

        public async Task SaveResetTokenAsync(UsuarioResetToken token)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("SaveResetToken");
                var param = new
                {
                    UsuarioId = token.UsuarioId,
                    Token = token.Token,
                    DataExpiracao = token.DataExpiracao
                };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar token de reset de senha!" + ex.Message);
            }
        }

        public async Task<UsuarioResetToken?> GetResetTokenAsync(string token)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetResetToken");
                var param = new { Token = token };

                var queryResult = await _repositoryDapper.QueryAsync<UsuarioResetToken>(sql, param);

                return queryResult.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter token de reset de senha!" + ex.Message);
            }
        }

        public async Task InvalidateResetTokenAsync(Guid tokenId)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("InvalidateResetToken");
                var param = new { Id = tokenId };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao invalidar token de reset de senha!" + ex.Message);
            }
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPasswordHash)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateUserPassword");
                var param = new { Id = userId, SenhaHash = newPasswordHash };

                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar senha do usuário!" + ex.Message);
            }
        }
    }
}
