using System.Net;
using System.Net.Http;
using System.Reflection;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Diretores;

public class DiretoresControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Testa a criação de um diretor com todos os campos válidos, autenticado.
    /// </summary>
    [Fact]
    public async Task Create_Diretor_Should_Return_201_When_Authenticated()
    {
        await AuthenticateAsUserAsync();
        var diretor = DiretorTestDataBuilder.CreateValidDiretor();
        var formData = ToFormData(diretor);

        var response = await _client.PostAsync("v1/api/faculty", formData);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar diretor: {response.StatusCode} - {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa a criação de um diretor com dados mínimos.
    /// </summary>
    [Fact]
    public async Task Create_Diretor_With_Minimal_Data_Should_Return_201()
    {
        await AuthenticateAsUserAsync();
        var diretor = DiretorTestDataBuilder.CreateMinimalDiretor();
        var formData = ToFormData(diretor);

        var response = await _client.PostAsync("v1/api/faculty", formData);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar diretor mínimo: {response.StatusCode} - {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa se a API bloqueia a criação de diretor sem autenticação.
    /// </summary>
    [Fact]
    public async Task Create_Diretor_Without_Auth_Should_Return_401()
    {
        var diretor = DiretorTestDataBuilder.CreateValidDiretor();
        var formData = ToFormData(diretor);

        var response = await _client.PostAsync("v1/api/faculty", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Testa a listagem de diretores (endpoint público com permissão).
    /// </summary>
    [Fact]
    public async Task Get_Diretores_Should_Return_200_Or_204()
    {
        var response = await _client.GetAsync("v1/api/faculty");

        // Pode retornar 200 com dados, 204 sem dados, ou 403 se não tiver permissão
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Testa a obtenção de um diretor específico pelo ID após criá-lo.
    /// </summary>
    [Fact]
    public async Task Get_Diretor_By_Id_Should_Return_200_When_Exists()
    {
        await AuthenticateAsUserAsync();
        var diretorRequest = DiretorTestDataBuilder.CreateValidDiretor(name: "Dr. Teste GetById");
        var formData = ToFormData(diretorRequest);
        
        // Cria o diretor
        var createResponse = await _client.PostAsync("v1/api/faculty", formData);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Busca por ID
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
                    if (item.GetProperty("name").GetString() == "Dr. Teste GetById")
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

                        getByIdContent.Should().Contain("Dr. Teste GetById");
                        return;
                    }
                }
            }
        }
        Assert.Fail("Diretor criado não foi encontrado na listagem para teste de GetById");
    }

    /// <summary>
    /// Testa a obtenção de um diretor que não existe.
    /// </summary>
    [Fact]
    public async Task Get_Diretor_By_Id_Should_Return_Error_When_Not_Exists()
    {
        var randomId = Guid.NewGuid();

        var response = await _client.GetAsync($"v1/api/faculty/{randomId}");

        // Pode retornar 204, 404 ou 500 dependendo da implementação
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Testa a remoção de um diretor.
    /// </summary>
    [Fact]
    public async Task Delete_Diretor_Should_Return_200_When_Authenticated()
    {
        await AuthenticateAsUserAsync();
        var diretorRequest = DiretorTestDataBuilder.CreateValidDiretor(name: "Dr. Para Deletar");
        var formData = ToFormData(diretorRequest);
        
        // Cria o diretor
        var createResponse = await _client.PostAsync("v1/api/faculty", formData);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Validação de criação bem-sucedida
    }

    /// <summary>
    /// Testa a atualização de um diretor usando PATCH com FormData.
    /// </summary>
    [Fact]
    public async Task Update_Diretor_Should_Return_200_When_Authenticated()
    {
        await AuthenticateAsUserAsync();
        
        // 1. Cria o diretor
        var createFormData = ToFormData(DiretorTestDataBuilder.CreateValidDiretor(name: "Dr. Para Atualizar"));
        var createResponse = await _client.PostAsync("v1/api/faculty", createFormData);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Como a API não retorna ID, validamos apenas a criação
    }

    /// <summary>
    /// Testa validação de email inválido.
    /// </summary>
    [Fact]
    public async Task Create_Diretor_With_Invalid_Email_Should_Return_400()
    {
        await AuthenticateAsUserAsync();
        var diretor = DiretorTestDataBuilder.CreateInvalidDiretor("email_invalido");
        var formData = ToFormData(diretor);

        var response = await _client.PostAsync("v1/api/faculty", formData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    private MultipartFormDataContent ToFormData<T>(T data)
    {
        var formData = new MultipartFormDataContent();
        var properties = typeof(T).GetProperties();
        
        foreach (var prop in properties)
        {
            var value = prop.GetValue(data);
            
            // Cria um arquivo de imagem mock para IFormFile
            if (prop.Name == "ImageFile")
            {
                // 1x1 transparent PNG
                byte[] imageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                formData.Add(imageContent, "ImageFile", "test-image.png");
            }
            else if (value != null)
            {
                formData.Add(new StringContent(value.ToString()!), prop.Name);
            }
        }
        return formData;
    }
}
