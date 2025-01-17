using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DaccApi.Infrastructure.Dapper
{
    public class RepositoryDapper : IRepositoryDapper
    {
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

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await GetConnection().ExecuteAsync(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await GetConnection().QueryAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters = null)
        {
            return await GetConnection().QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
