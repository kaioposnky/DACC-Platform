using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;

namespace DaccApi.Tests;

public class IntegrationTestBase : IAsyncLifetime
{
    private PostgreSqlContainer _dbcontainer = null!;
    protected HttpClient _client = null!;
    private WebApplicationFactory<Program> _factory = null!;

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

    public async Task DisposeAsync()
    {
        await _dbcontainer.StopAsync();
        await _dbcontainer.DisposeAsync();
        _client.Dispose();
    }
}
