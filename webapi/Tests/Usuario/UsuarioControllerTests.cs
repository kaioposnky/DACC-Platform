using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DaccApi.Model;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Usuario;

public class UsuarioControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/users";

    [Fact]
    public async Task Get_Users_Should_Return_List_When_Admin()
    {
        await AuthenticateAsAdminAsync();
        
        var response = await _client.GetAsync(BaseUrl);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        // Verifica se contém o email do admin que acabamos de criar/logar
        content.Should().Contain(TestAdminEmail);
    }
    
    [Fact]
    public async Task Get_User_By_Id_Should_Return_Data_When_Exists()
    {
        await AuthenticateAsAdminAsync();
        var adminId = await GetUserIdByEmail(TestAdminEmail);
        
        var response = await _client.GetAsync($"{BaseUrl}/{adminId}");
        
        // Controller tem bug conhecido que retorna 200 OK mas não retorna o objeto no data.
        // Se esse teste falhar dizendo que não encontrou o email no content, confirmamos o bug.
        // Se passar, o bug não existe.
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        
        // Se o bug existir, o content será algo como { ..., "data": "Usuário obtido com sucesso!", ... } ou data null
        // Se estiver corrigido, deve conter o email.
        content.Should().Contain(TestAdminEmail);
    }

    [Fact]
    public async Task Update_User_Should_Work()
    {
        await AuthenticateAsAdminAsync();
        var adminId = await GetUserIdByEmail(TestAdminEmail);
        
        var updateRequest = UsuarioTestDataBuilder.CreateUpdateUsuario("Admin Updated");
        var formData = ToFormData(updateRequest);
        
        var response = await _client.PatchAsync($"{BaseUrl}/{adminId}", formData);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verifica se atualizou (novamente, depende do GetById funcionar ou do retorno do Update)
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Admin Updated");
    }

    [Fact]
    public async Task Delete_User_Should_Work()
    {
        await AuthenticateAsAdminAsync();
        // Precisamos criar um usuário extra para deletar, não podemos deletar o próprio admin logado (ou será que pode?)
        // Vamos criar um user aluno via Auth, pegar ID e deletar.
        
        // Criar aluno
        var alunoEmail = "aluno_del@dacc.com";
        var registerRequest = new
        {
            nome = "Aluno",
            sobrenome = "Delete",
            email = alunoEmail,
            ra = "111222333",
            curso = "CC",
            telefone = "11999999999",
            senha = "Password123",
            inscritoNoticia = false
        };
        await _client.PostAsJsonAsync("v1/api/auth/register", registerRequest);
        
        // Pegar ID do aluno (precisamos logar como admin para listar users e achar ID)
        var listResponse = await _client.GetAsync(BaseUrl);
        var listContent = await listResponse.Content.ReadAsStringAsync();
        var alunoId = GetIdFromList(listContent, alunoEmail);
        
        alunoId.Should().NotBeNull();
        
        // Deletar
        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{alunoId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verificar se sumiu
        var checkResponse = await _client.GetAsync($"{BaseUrl}/{alunoId}");
        // Devido ao bug do GetById, pode retornar 204/404 ou 200 sem data.
        // Se o delete funcionou, GetById deve retornar 404 (ResourceNotFound).
        checkResponse.StatusCode.Should().Be(HttpStatusCode.NotFound); 
    }

    private async Task<Guid> GetUserIdByEmail(string email)
    {
        var listResponse = await _client.GetAsync(BaseUrl);
        var content = await listResponse.Content.ReadAsStringAsync();
        return GetIdFromList(content, email) ?? throw new Exception($"User {email} not found in list");
    }

    private Guid? GetIdFromList(string jsonContent, string email)
    {
        using var doc = JsonDocument.Parse(jsonContent);
        var root = doc.RootElement;
        
        // Unwrapping data
        if (!root.TryGetProperty("data", out var dataElement)) return null;
        
        JsonElement usersList = dataElement;
        if (dataElement.ValueKind == JsonValueKind.Object && dataElement.TryGetProperty("users", out var inner))
            usersList = inner;
            
        if (usersList.ValueKind != JsonValueKind.Array) return null;

        foreach (var item in usersList.EnumerateArray())
        {
            if (item.TryGetProperty("email", out var e) && e.GetString() == email)
            {
                if (item.TryGetProperty("id", out var id))
                    return id.GetGuid();
            }
        }
        return null;
    }

    private MultipartFormDataContent ToFormData(RequestUpdateUsuario request)
    {
        var formData = new MultipartFormDataContent();
        if (request.Name != null) formData.Add(new StringContent(request.Name), "Name");
        if (request.LastName != null) formData.Add(new StringContent(request.LastName), "LastName");
        if (request.Phone != null) formData.Add(new StringContent(request.Phone), "Phone");
        if (request.Course != null) formData.Add(new StringContent(request.Course), "Course");
        
        return formData;
    }
}
