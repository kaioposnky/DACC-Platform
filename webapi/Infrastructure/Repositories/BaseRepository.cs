using System.Reflection;
using DaccApi.Infrastructure.Dapper;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaccApi.Data.Orm
{
    /// <summary>
    /// Classe base abstrata para repositórios que implementam operações CRUD básicas reutilizando o IRepositoryDapper.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade que este repositório gerencia.</typeparam>
    public abstract class BaseRepository<T> where T : new()
    {
        /// <summary>
        /// Instância do repositório Dapper para execução de comandos.
        /// </summary>
        private readonly IRepositoryDapper _dapper;
        private readonly string _tableName;
        private readonly PropertyInfo[] _properties;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="BaseRepository{T}"/> inferindo o nome da tabela via atributo ou nome da classe.
        /// </summary>
        /// <param name="dapper">O repositório Dapper injetado.</param>
        protected BaseRepository(IRepositoryDapper dapper) 
            : this(dapper, typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name.ToLower())
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="BaseRepository{T}"/>.
        /// </summary>
        /// <param name="dapper">O repositório Dapper injetado.</param>
        /// <param name="tableName">O nome da tabela no banco de dados.</param>
        protected BaseRepository(IRepositoryDapper dapper, string tableName)
        {
            _dapper = dapper;
            _tableName = tableName;

            // Cache das propriedades para não fazer reflection toda hora
            _properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null)
                .ToArray();
        }

        /// <summary>
        /// Obtém todas as entidades da tabela de forma assíncrona.
        /// </summary>
        /// <returns>Uma lista de entidades do tipo <typeparamref name="T"/>.</returns>
        public async Task<List<T>> GetAllAsync()
        {
            // Dapper com MatchNamesWithUnderscores = true faz o mapeamento automático
            var sql = $"SELECT * FROM \"{_tableName}\"";
            var result = await _dapper.QueryAsync<T>(sql);
            return result.ToList();
        }

        /// <summary>
        /// Obtém uma entidade pelo seu ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID da entidade a ser buscada.</param>
        /// <returns>A entidade encontrada ou null se não existir.</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM \"{_tableName}\" WHERE \"id\" = @Id";
            // Usamos QueryAsync + FirstOrDefault para evitar exceção se não encontrar (QueryFirstAsync lança erro)
            var result = await _dapper.QueryAsync<T>(sql, new { Id = id });
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Cria uma nova entidade no banco de dados de forma assíncrona.
        /// </summary>
        /// <param name="entity">A entidade a ser criada.</param>
        /// <returns>Verdadeiro se a criação for bem-sucedida, falso caso contrário.</returns>
        public async Task<bool> CreateAsync(T entity)
        {
            var cols = new List<string>();
            var paramsNames = new List<string>();

            foreach (var prop in _properties)
            {
                var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
                var colName = colAttr?.Name ?? prop.Name;
                
                cols.Add($"\"{colName}\"");
                // Dapper mapeia @NomePropriedade para o valor da propriedade no objeto
                paramsNames.Add("@" + prop.Name);
            }

            var sql = $"INSERT INTO \"{_tableName}\" ({string.Join(",", cols)}) VALUES ({string.Join(",", paramsNames)})";
            
            var rows = await _dapper.ExecuteAsync(sql, entity);
            return rows > 0;
        }

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID da entidade a ser atualizada.</param>
        /// <param name="entity">Os novos dados da entidade.</param>
        /// <returns>Verdadeiro se a atualização for bem-sucedida, falso caso contrário.</returns>
        public async Task<bool> UpdateAsync(Guid id, T entity)
        {
            var updates = new List<string>();

            // Garante que o ID do objeto seja o mesmo do parâmetro para o WHERE funcionar
            var idProp = _properties.FirstOrDefault(p => p.Name == "Id");
            idProp?.SetValue(entity, id);

            foreach (var prop in _properties)
            {
                var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
                var colName = colAttr?.Name ?? prop.Name;

                if (colName.ToLower() == "id") continue; // Não atualiza ID
                if (colName.ToLower() == "data_criacao") continue; // Não atualiza data de criação

                updates.Add($"\"{colName}\" = @{prop.Name}");
            }

            var sql = $"UPDATE \"{_tableName}\" SET {string.Join(",", updates)} WHERE \"id\" = @Id";

            var rows = await _dapper.ExecuteAsync(sql, entity);
            return rows > 0;
        }

        /// <summary>
        /// Remove uma entidade do banco de dados pelo seu ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID da entidade a ser removida.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida, falso caso contrário.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM \"{_tableName}\" WHERE \"id\" = @Id";
            var rows = await _dapper.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}