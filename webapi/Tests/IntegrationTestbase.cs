using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using Xunit;

namespace DaccApi.Tests;

public class IntegrationTestBase : IAsyncLifetime
{
    private PostgreSqlContainer _dbcontainer = null!;
    protected HttpClient _client = null!;
    private WebApplicationFactory<Program> _factory = null!;

    // Dados de teste para usuários
    protected const string TestUserEmail = "test@dacc.com";
    protected const string TestUserPassword = "Test@123";
    protected const string TestAdminEmail = "admin@dacc.com";
    protected const string TestAdminPassword = "Admin@123";

    public async Task InitializeAsync()
    {

        _dbcontainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("dacc_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .Build();
        await _dbcontainer.StartAsync();

        await ExecuteSqlScriptAsync(_dbcontainer.GetConnectionString());

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // Define o ContentRootPath para a raiz do projeto
                var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
                builder.UseContentRoot(projectRoot);
                
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // Sobrescreve a connection string para usar o container
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["ConnectionStrings:DefaultConnection"] = _dbcontainer.GetConnectionString(),
                        ["UploadFilesSubfolder"] = "uploads" // Configuração necessária
                    }!);
                });
            });

        _client = _factory.CreateClient();
    }

    private async Task ExecuteSqlScriptAsync(string connectionString)
    {
        // Caminho para o arquivo SQL na raiz do projeto
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var sqlFilePath = Path.Combine(projectRoot, "sqlcode.sql");
        
        if (!File.Exists(sqlFilePath))
        {
            throw new FileNotFoundException($"Arquivo SQL não encontrado: {sqlFilePath}");
        }

        var sqlScript = await File.ReadAllTextAsync(sqlFilePath);

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sqlScript, connection);
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Autentica o client como um usuário normal e retorna o token
    /// </summary>
    protected async Task<string> AuthenticateAsUserAsync()
    {
        await CreateTestUserIfNotExistsAsync(TestUserEmail, TestUserPassword, "aluno");
        var token = await LoginAsync(TestUserEmail, TestUserPassword);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    /// <summary>
    /// Autentica o client como administrador e retorna o token
    /// </summary>
    protected async Task<string> AuthenticateAsAdminAsync()
    {
        await CreateTestUserIfNotExistsAsync(TestAdminEmail, TestAdminPassword, "administrador");
        var token = await LoginAsync(TestAdminEmail, TestAdminPassword);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    /// <summary>
    /// Remove a autenticação do client
    /// </summary>
    protected void ClearAuthentication()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }

    private async Task CreateTestUserIfNotExistsAsync(string email, string password, string cargo)
    {
        var registerRequest = new
        {
            nome = "Test",
            sobrenome = "User",
            email = email,
            ra = cargo == "administrador" ? "999999999" : "123456789", // 9 dígitos conforme validação
            curso = "Ciência da Computação",
            telefone = "11999999999",
            senha = password,
            inscritoNoticia = false
        };

        var response = await _client.PostAsJsonAsync("v1/api/auth/register", registerRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine($"[DEBUG] Register response status: {response.StatusCode}");
        Console.WriteLine($"[DEBUG] Register response: {responseContent}");
        
        // Se deu erro diferente de BadRequest (que pode ser email já existe), lança exceção detalhada
        if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.BadRequest)
        {
            throw new Exception($"Erro ao criar usuário ({response.StatusCode}): {responseContent}");
        }
        
        // Se for admin, atualiza o cargo no banco
        if (cargo == "administrador" && response.IsSuccessStatusCode)
        {
            await SetUserRoleDirectlyAsync(email, cargo);
        }
    }

    private async Task SetUserRoleDirectlyAsync(string email, string cargo)
    {
        var connectionString = _dbcontainer.GetConnectionString();
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        var sql = "UPDATE usuario SET cargo = @cargo WHERE email = @email";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("cargo", cargo);
        command.Parameters.AddWithValue("email", email);
        await command.ExecuteNonQueryAsync();
    }

    private async Task<string> LoginAsync(string email, string password)
    {
        var loginRequest = new { email, senha = password };
        var response = await _client.PostAsJsonAsync("v1/api/auth/login", loginRequest);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login falhou ({response.StatusCode}): {errorContent}");
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseWrapper>();
        return loginResponse?.Data?.AccessToken ?? throw new Exception("Token não recebido");
    }

    private class LoginResponseWrapper
    {
        public LoginData? Data { get; set; }
    }

    private class LoginData
    {
        public string? AccessToken { get; set; }
    }

    /// <summary>
    /// Busca o GUID de uma categoria pelo nome
    /// </summary>
    protected async Task<string> GetCategoriaIdAsync(string nome)
    {
        var connectionString = _dbcontainer.GetConnectionString();
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        var sql = "SELECT id FROM produto_categoria WHERE nome = @nome";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("nome", nome);
        
        var result = await command.ExecuteScalarAsync();
        return result?.ToString() ?? throw new Exception($"Categoria '{nome}' não encontrada");
    }

    /// <summary>
    /// Busca o GUID de uma subcategoria pelo nome
    /// </summary>
    protected async Task<string> GetSubcategoriaIdAsync(string nome)
    {
        var connectionString = _dbcontainer.GetConnectionString();
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        var sql = "SELECT id FROM produto_subcategoria WHERE nome = @nome";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("nome", nome);
        
        var result = await command.ExecuteScalarAsync();
        return result?.ToString() ?? throw new Exception($"Subcategoria '{nome}' não encontrada");
    }

    public async Task DisposeAsync()
    {
        await _dbcontainer.StopAsync();
        await _dbcontainer.DisposeAsync();
        _client.Dispose();
    }
}
