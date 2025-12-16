using System.Data;

namespace DaccApi.Infrastructure.Dapper
{
    /// <summary>
    /// Define a interface para um repositório genérico usando Dapper.
    /// </summary>
    public interface IRepositoryDapper
    {
        /// <summary>
        /// Executa um comando SQL assíncrono dentro de uma transação.
        /// </summary>
        Task<int> ExecuteAsync(string sql, object? parameters, IDbTransaction transaction);
        /// <summary>
        /// Executa uma stored procedure assíncrona dentro de uma transação.
        /// </summary>
        Task<int> ExecuteProcedureAsync(string procedureName, object? parameters, IDbTransaction transaction);
        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona dentro de uma transação.
        /// </summary>
        Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters, IDbTransaction transaction);
        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona e retorna o primeiro resultado dentro de uma transação.
        /// </summary>
        Task<T> QueryProcedureFirstAsync<T>(string procedureName, object? parameters, IDbTransaction transaction);
        /// <summary>
        /// Executa uma consulta SQL assíncrona dentro de uma transação.
        /// </summary>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, IDbTransaction transaction);
        /// <summary>
        /// Inicia uma nova transação de banco de dados.
        /// </summary>
        IDbTransaction BeginTransaction();
        /// <summary>
        /// Executa um comando SQL assíncrono.
        /// </summary>
        Task<int> ExecuteAsync(string sql, object? parameters = null);
        /// <summary>
        /// Executa uma consulta SQL síncrona.
        /// </summary>
        IEnumerable<T> Query<T>(string sql, object? parameters = null);
        /// <summary>
        /// Executa uma consulta SQL assíncrona.
        /// </summary>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        /// <summary>
        /// Executa uma consulta SQL assíncrona e retorna o primeiro resultado.
        /// </summary>
        Task<T> QueryFirstAsync<T>(string sql, object? parameters = null);
        /// <summary>
        /// Executa uma stored procedure de consulta assíncrona.
        /// </summary>
        Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object? parameters = null);
        /// <summary>
        /// Obtém uma nova conexão com o banco de dados.
        /// </summary>
        IDbConnection GetConnection();
        /// <summary>
        /// Obtém uma consulta SQL nomeada a partir dos arquivos de configuração XML.
        /// </summary>
        string GetQueryNamed(string queryName);
    }
}
