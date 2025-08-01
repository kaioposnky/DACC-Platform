using System.Data;

namespace DaccApi.Infrastructure.Dapper
{
    public interface IRepositoryDapper
    {
        Task<int> ExecuteAsync(string sql, object? parameters = null);
        IEnumerable<T> Query<T>(string sql, object? parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters = null);
        IDbConnection GetConnection();
        string GetQueryNamed(string queryName);
    }
}
