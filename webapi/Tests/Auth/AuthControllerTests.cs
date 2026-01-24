using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DaccApi.Model;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Auth;

/// <summary>
/// Testes de integração para AuthController.
/// </summary>
public class AuthControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/auth";

    /// <summary>
    /// Testa o registro de um usuário novo com sucesso.
    /// </summary>
    [Fact]
    public async Task Register_User_Should_Return_201()
    {
        // Garante email e RA únicos
        var uniqueSuffix = Guid.NewGuid().ToString().Substring(0, 8);
        // RA precisa ser numérico e 9 digitos? Validação diz 9 chars? Vamos usar um RA válido
        // IntegrationTestBase usa "123456789" para aluno padrão. Vamos usar um diferente.
        var ra = "987654321"; // Outro RA válido
        
        // Preciso garantir que não existe no banco, mas como base de teste sobe limpa/mockada ou compartilha?
        // Container é compartilhado na sessão de teste.
        // Vamos usar um RA gerado para evitar conflito se o teste rodar várias vezes ou em paralelo
        // Mas RA tem validação de formato? [RaValidation]. Geralmente aceita números.
        
        var request = AuthTestDataBuilder.CreateValidRegister($"newuser_{uniqueSuffix}@dacc.com", "987654321");
        
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/register", request);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao registrar: {response.StatusCode} - {error}");
        }
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Verifica se retornou ID ou objeto user? (Reportado como bug em reviews, vamos verificar o behavior atual)
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("newuser_");
    }

    /// <summary>
    /// Testa login com sucesso.
    /// </summary>
    [Fact]
    public async Task Login_Should_Return_Token_When_Credentials_Valid()
    {
        // 1. Cria usuário (usando helper da Base que já cria e verifica)
        var email = $"login_test_{Guid.NewGuid()}@dacc.com";
        var password = "Strong@Password1";
        await CreateTestUserIfNotExistsAsync(email, password, "aluno");

        // 2. Tenta logar
        var loginRequest = AuthTestDataBuilder.CreateValidLogin(email, password);
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        // Deve conter accessToken
        content.Should().Contain("accessToken");
        content.Should().Contain("refreshToken");
    }

    /// <summary>
    /// Testa login com senha inválida.
    /// </summary>
    [Fact]
    public async Task Login_Should_Return_401_When_Password_Invalid()
    {
        var email = $"inv_pass_{Guid.NewGuid()}@dacc.com";
        await CreateTestUserIfNotExistsAsync(email, "Correct@123", "aluno");

        var loginRequest = AuthTestDataBuilder.CreateValidLogin(email, "Wrong@123");
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/login", loginRequest);

        // API pode retornar 401 ou 400 ou 404 dependendo da implementação de segurança
        // Geralmente 400 Credentials Invalid ou 401
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest);
    }
    
    /// <summary>
    /// Testa registro duplicado (mesmo email).
    /// </summary>
    [Fact]
    public async Task Register_Duplicate_Email_Should_Fail()
    {
        var email = $"dup_email_{Guid.NewGuid()}@dacc.com";
        var request1 = AuthTestDataBuilder.CreateValidRegister(email, "111222333");
        
        // Primeiro registro
        var res1 = await _client.PostAsJsonAsync($"{BaseUrl}/register", request1);
        res1.StatusCode.Should().Be(HttpStatusCode.Created);

        // Segundo registro identico (exceto RA talvez, pra isolar erro de email)
        var request2 = AuthTestDataBuilder.CreateValidRegister(email, "444555666");
        var res2 = await _client.PostAsJsonAsync($"{BaseUrl}/register", request2);
        
        res2.StatusCode.Should().NotBe(HttpStatusCode.Created);
        // Geralmente 400 Bad Request com msg "Email já cadastrado" ou 500 se estourar constraint
        res2.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
        
        var content = await res2.Content.ReadAsStringAsync();
        content.Should().ContainEquivalentOf("email"); // Mensagem deve citar email
    }
}
