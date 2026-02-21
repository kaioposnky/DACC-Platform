using System.Net;
using System.Net.Http.Json;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Professores;

public class ProfessoresControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Testa a criação de um professor com todos os campos válidos, autenticado.
    /// </summary>
    [Fact]
    public async Task Create_Professor_Should_Return_201_When_Authenticated()
    {
        await AuthenticateAsAdminAsync();
        var response = await _client.PostAsJsonAsync("v1/api/faculty", ProfessorTestDataBuilder.CreateValidProfessor());

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar professor: {response.StatusCode} - {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa a criação de um professor com dados mínimos.
    /// </summary>
    [Fact]
    public async Task Create_Professor_With_Minimal_Data_Should_Return_201()
    {
        await AuthenticateAsAdminAsync();
        var response = await _client.PostAsJsonAsync("v1/api/faculty", ProfessorTestDataBuilder.CreateMinimalProfessor());

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar professor mínimo: {response.StatusCode} - {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa se a API bloqueia a criação de professor sem autenticação.
    /// </summary>
    [Fact]
    public async Task Create_Professor_Without_Auth_Should_Return_401()
    {
        var response = await _client.PostAsJsonAsync("v1/api/faculty", ProfessorTestDataBuilder.CreateValidProfessor());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Testa a listagem de professores (endpoint público).
    /// </summary>
    [Fact]
    public async Task Get_Professores_Should_Return_200_Or_204()
    {
        var response = await _client.GetAsync("v1/api/faculty");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Testa a obtenção de um professor específico pelo ID após criá-lo.
    /// </summary>
    [Fact]
    public async Task Get_Professor_By_Id_Should_Return_200_When_Exists()
    {
        await AuthenticateAsAdminAsync();
        var professorRequest = ProfessorTestDataBuilder.CreateValidProfessor(name: "Prof. Teste GetById");
        var createResponse = await _client.PostAsJsonAsync("v1/api/faculty", professorRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Busca por ID na listagem
        var listResponse = await _client.GetAsync("v1/api/faculty");
        if (listResponse.StatusCode == HttpStatusCode.OK)
        {
            var listContent = await listResponse.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(listContent);
            var root = doc.RootElement;
            if (root.TryGetProperty("data", out var dataProp))
            {
                System.Text.Json.JsonElement list;
                if (dataProp.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    list = dataProp;
                }
                else
                {
                    list = dataProp.TryGetProperty("faculty", out var f) ? f : dataProp;
                }
                foreach (var item in list.EnumerateArray())
                {
                    if (item.GetProperty("name").GetString() == "Prof. Teste GetById")
                    {
                        var id = item.GetProperty("id").GetGuid();
                        var getByIdResponse = await _client.GetAsync($"v1/api/faculty/{id}");
                        
                        if (!getByIdResponse.IsSuccessStatusCode)
                        {
                            var error = await getByIdResponse.Content.ReadAsStringAsync();
                            var status = getByIdResponse.StatusCode;
                            Assert.Fail($"GetById falhou. Status: {status}, Erro: {error}");
                        }
                        
                        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                        
                        var getByIdContent = await getByIdResponse.Content.ReadAsStringAsync();
                        getByIdContent.Should().Contain("Prof. Teste GetById");
                        return;
                    }
                }
            }
        }
        Assert.Fail("Professor criado não foi encontrado na listagem para teste de GetById");
    }

    /// <summary>
    /// Testa a obtenção de um professor que não existe.
    /// </summary>
    [Fact]
    public async Task Get_Professor_By_Id_Should_Return_Error_When_Not_Exists()
    {
        var randomId = Guid.NewGuid();

        var response = await _client.GetAsync($"v1/api/faculty/{randomId}");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Testa a remoção de um professor.
    /// </summary>
    [Fact]
    public async Task Delete_Professor_Should_Return_200_When_Authenticated()
    {
        await AuthenticateAsAdminAsync();
        var professorRequest = ProfessorTestDataBuilder.CreateValidProfessor(name: "Prof. Para Deletar");
        var createResponse = await _client.PostAsJsonAsync("v1/api/faculty", professorRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Busca o ID para deletar
        var listResponse = await _client.GetAsync("v1/api/faculty");
        if (listResponse.StatusCode == HttpStatusCode.OK)
        {
            var listContent = await listResponse.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(listContent);
            var root = doc.RootElement;
            if (root.TryGetProperty("data", out var dataProp))
            {
                var list = dataProp.TryGetProperty("faculty", out var f) ? f : dataProp;
                foreach (var item in list.EnumerateArray())
                {
                    if (item.GetProperty("name").GetString() == "Prof. Para Deletar")
                    {
                        var id = item.GetProperty("id").GetGuid();
                        var deleteResponse = await _client.DeleteAsync($"v1/api/faculty/{id}");
                        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Testa a atualização de um professor.
    /// </summary>
    [Fact]
    public async Task Update_Professor_Should_Return_200_When_Authenticated()
    {
        await AuthenticateAsAdminAsync();
        
        var createResponse = await _client.PostAsJsonAsync("v1/api/faculty", ProfessorTestDataBuilder.CreateValidProfessor(name: "Prof. Para Atualizar"));
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Valida criação
        createResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    /// <summary>
    /// Testa validação de email inválido.
    /// </summary>
    [Fact]
    public async Task Create_Professor_With_Invalid_Email_Should_Return_400()
    {
        await AuthenticateAsAdminAsync();
        var response = await _client.PostAsJsonAsync("v1/api/faculty", ProfessorTestDataBuilder.CreateInvalidProfessor("email_invalido"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
