using System.Data;
using System.Xml.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DaccApi.Infrastructure.Dapper
{
    /// <summary>
    /// Implementação de um repositório genérico usando Dapper para operações de banco de dados.
    /// </summary>
    public class RepositoryDapper : IRepositoryDapper, IDisposable
    {
        private Dictionary<string, string> _queriesCache = new Dictionary<string, string>();
        private readonly string _connectionString;
        private bool _disposed = false;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RepositoryDapper"/>.
        /// </summary>
        public RepositoryDapper(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        /// <summary>
        /// Obtém uma nova conexão com o banco de dados.
        /// </summary>
        public IDbConnection GetConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// Executa uma stored procedure assíncrona dentro de uma transação.
        /// </summary>
        public async Task<int> ExecuteProcedureAsync(
            string procedureName,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.ExecuteAsync(
                procedureName,
                parameters,
                transaction,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona dentro de uma transação.
        /// </summary>
        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(
            string procedureName,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.QueryAsync<T>(
                procedureName,
                parameters,
                transaction,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona e retorna o primeiro resultado dentro de uma transação.
        /// </summary>
        public async Task<T> QueryProcedureFirstAsync<T>(
            string procedureName,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.QueryFirstAsync<T>(
                procedureName,
                parameters,
                transaction,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Executa um comando SQL assíncrono dentro de uma transação.
        /// </summary>
        public async Task<int> ExecuteAsync(
            string sql,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.ExecuteAsync(sql, parameters, transaction);
        }

        /// <summary>
        /// Executa uma consulta SQL assíncrona dentro de uma transação.
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.QueryAsync<T>(sql, parameters, transaction);
        }

        /// <summary>
        /// Inicia uma nova transação de banco de dados.
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            var connection = GetConnection();
            connection.Open();
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Executa um comando SQL assíncrono.
        /// </summary>
        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Executa uma consulta SQL síncrona.
        /// </summary>
        public IEnumerable<T> Query<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<T>(sql, parameters);
        }

        /// <summary>
        /// Executa uma consulta SQL assíncrona.
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        /// <summary>
        /// Executa uma consulta SQL assíncrona e retorna o primeiro resultado.
        /// </summary>
        public async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.QueryFirstAsync<T>(sql, parameters);
        }

        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona.
        /// </summary>
        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(
            string procedureName,
            object? parameters = null
        )
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.QueryAsync<T>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtém uma consulta SQL nomeada a partir dos arquivos de configuração XML.
        /// </summary>
        public string GetQueryNamed(string queryName)
        {
            try
            {
                if (_queriesCache.ContainsKey(queryName))
                {
                    return _queriesCache[queryName];
                }

                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Queries");

                var xmlFiles = Directory.GetFiles(directoryPath, "*.hbm.xml");

                foreach (var file in xmlFiles)
                {
                    var xdoc = XDocument.Load(file);
                    var queryElement = xdoc.Descendants("query")
                        .FirstOrDefault(q => q.Attribute("name")?.Value == queryName);

                    if (queryElement != null)
                    {
                        _queriesCache[queryName] = queryElement.Value.Trim();
                        return _queriesCache[queryName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Query '{queryName}' não encontrada: {ex.Message}");
            }
            throw new KeyNotFoundException(
                $"Query '{queryName}' não encontrada em nenhum arquivo de configuração."
            );
        }

        /// <summary>
        /// Libera os recursos utilizados pelo repositório.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera os recursos não gerenciados e, opcionalmente, os gerenciados.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _queriesCache?.Clear();
                }
                _disposed = true;
            }
        }
    }
}
