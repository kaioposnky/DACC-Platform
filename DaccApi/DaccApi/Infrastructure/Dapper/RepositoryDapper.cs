using System.Data;
using System.Xml.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DaccApi.Infrastructure.Dapper
{
    public class RepositoryDapper : IRepositoryDapper
    {
        private Dictionary<string, string> _queriesCache = new Dictionary<string, string>();
        private readonly string _connectionString;

        private IDbConnection? _connection;

        public RepositoryDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                 throw new InvalidOperationException("Connection string not found.");
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new Npgsql.NpgsqlConnection(_connectionString);
                OpenConnection();
            }
            return _connection!;
        }

        public void OpenConnection()
        {
            if (_connection?.State != ConnectionState.Open)
                _connection?.Open();
        }

        public void CloseConnection()
        {
            try
            {
                if (_connection?.State == ConnectionState.Open)
                    _connection?.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while closing connection", ex);
            }
        }

        public async Task<int> ExecuteProcedureAsync(string procedureName, object? parameters, IDbTransaction transaction)
        {
            return await transaction.Connection!.ExecuteAsync(procedureName, parameters, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters, IDbTransaction transaction)
        {
            return await transaction.Connection!.QueryAsync<T>(procedureName, parameters, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> QueryProcedureFirstAsync<T>(string procedureName, object? parameters, IDbTransaction transaction)
        {
            return await transaction.Connection!.QueryFirstAsync<T>(procedureName, parameters, transaction, commandType: CommandType.StoredProcedure);
        }
        
        public async Task<int> ExecuteAsync(string sql, object? parameters, IDbTransaction transaction)
        {
            return await transaction.Connection!.ExecuteAsync(sql, parameters, transaction);
        }
        
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, IDbTransaction transaction)
        {
            return await transaction.Connection!.QueryAsync<T>(sql, parameters, transaction);
        }

        public IDbTransaction BeginTransactionAsync()
        {
            return GetConnection().BeginTransaction();
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await GetConnection().ExecuteAsync(sql, parameters);
        }

        public IEnumerable<T> Query<T>(string sql, object? parameters = null)
        {
            return GetConnection().Query<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await GetConnection().QueryAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryFirstAsync<T>(string sql, object? parameters = null)
        {
            return await GetConnection().QueryAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters = null)
        {
            return await GetConnection().QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
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
                throw new KeyNotFoundException($"Query '{queryName}' não encontrada em nenhum arquivo de configuração.");
            }
            return ($"Query '{queryName}' não encontrada em nenhum arquivo de configuração.");
        }

    }
}
