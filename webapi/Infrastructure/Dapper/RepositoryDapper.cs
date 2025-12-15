using System.Data;
using System.Xml.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DaccApi.Infrastructure.Dapper
{
    public class RepositoryDapper : IRepositoryDapper, IDisposable
    {
        private Dictionary<string, string> _queriesCache = new Dictionary<string, string>();
        private readonly string _connectionString;
        private bool _disposed = false;

        public RepositoryDapper(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public IDbConnection GetConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString);
        }

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

        public async Task<int> ExecuteAsync(
            string sql,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters,
            IDbTransaction transaction
        )
        {
            return await transaction.Connection!.QueryAsync<T>(sql, parameters, transaction);
        }

        public IDbTransaction BeginTransaction()
        {
            var connection = GetConnection();
            connection.Open();
            return connection.BeginTransaction();
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.ExecuteAsync(sql, parameters);
        }

        public IEnumerable<T> Query<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return await connection.QueryFirstAsync<T>(sql, parameters);
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
