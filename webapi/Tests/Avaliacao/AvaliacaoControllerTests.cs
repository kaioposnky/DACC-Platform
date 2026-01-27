using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DaccApi.Model;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Avaliacao;

public class AvaliacaoControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/ratings";
    private const string ProductsUrl = "v1/api/products";

    /// <summary>
    /// Testa o fluxo completo de avaliação: criar, listar, atualizar e deletar.
    /// </summary>
    [Fact]
    public async Task Avaliacao_Full_Lifecycle_Should_Work()
    {
        await AuthenticateAsAdminAsync();

        // 1. Criar um produto para avaliar
        var product = ProductTestDataBuilder.CreateValidProduct(name: "Produto Para Avaliar");
        var productCreateResponse = await _client.PostAsJsonAsync(ProductsUrl, product);
        if (productCreateResponse.StatusCode != HttpStatusCode.Created)
        {
            var error = await productCreateResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar produto: {productCreateResponse.StatusCode} - {error}");
        }

        // 2. Obter o ID do produto criado
        Guid? produtoId = null;
        var listProductsResponse = await _client.GetAsync(ProductsUrl);
        if (listProductsResponse.StatusCode == HttpStatusCode.OK)
        {
            var content = await listProductsResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            // Lidando com unwrapping do ResponseSuccess
            JsonElement dataElement = root.GetProperty("data");
            JsonElement productsList = dataElement;
            if (dataElement.ValueKind == JsonValueKind.Object && dataElement.TryGetProperty("products", out var inner))
                productsList = inner;

            foreach (var item in productsList.EnumerateArray())
            {
                if (item.GetProperty("name").GetString() == "Produto Para Avaliar")
                {
                    produtoId = item.GetProperty("id").GetGuid();
                    break;
                }
            }
        }

        produtoId.Should().NotBeNull("Deve ser possível encontrar o produto criado para avaliar");

        // 3. Criar uma avaliação
        var avaliacao = AvaliacaoTestDataBuilder.CreateValidAvaliacao(produtoId!.Value, 5, "Produto sensacional!");
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, avaliacao);
        
        if (!createResponse.IsSuccessStatusCode)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar avaliação: {createResponse.StatusCode} - {error}");
        }
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // 4. Listar avaliações do produto
        var getByProductResponse = await _client.GetAsync($"{BaseUrl}/products/{produtoId}");
        getByProductResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var ratingsContent = await getByProductResponse.Content.ReadAsStringAsync();
        ratingsContent.Should().Contain("Produto sensacional!");

        // 5. Obter ID da avaliação para Update/Delete
        Guid? avaliacaoId = null;
        using (var ratingsDoc = JsonDocument.Parse(ratingsContent))
        {
            var data = ratingsDoc.RootElement.GetProperty("data");
            JsonElement ratingsList = data;
            // Verificar se está envelopado em "avaliacoes" ou similar
            if (data.ValueKind == JsonValueKind.Object && data.TryGetProperty("reviews", out var inner))
                ratingsList = inner;
            
            foreach (var item in ratingsList.EnumerateArray())
            {
                if (item.TryGetProperty("comment", out var c) && c.GetString() == "Produto sensacional!")
                {
                    avaliacaoId = item.GetProperty("id").GetGuid();
                    break;
                }
            }
        }
        
        avaliacaoId.Should().NotBeNull("Deve encontrar o ID da avaliação criada");

        // 6. Atualizar a avaliação
        var updateRequest = AvaliacaoTestDataBuilder.CreateUpdateAvaliacao(4, "Bom, mas pode melhorar.");
        var updateResponse = await _client.PatchAsJsonAsync($"{BaseUrl}/{avaliacaoId}", updateRequest);
        
        if (!updateResponse.IsSuccessStatusCode)
        {
             var error = await updateResponse.Content.ReadAsStringAsync();
             Assert.Fail($"Patch avaliacao falhou. Status: {updateResponse.StatusCode}, Erro: {error}");
        }
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 7. Deletar a avaliação
        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{avaliacaoId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 8. Verificar se foi deletada
        var checkResponse = await _client.GetAsync($"{BaseUrl}/{avaliacaoId}");
        checkResponse.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_Avaliacao_Without_Auth_Should_Return_401()
    {
        var avaliacao = AvaliacaoTestDataBuilder.CreateValidAvaliacao(Guid.NewGuid());
        var response = await _client.PostAsJsonAsync(BaseUrl, avaliacao);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Testa a listagem de todas as avaliações (requer permissão admin/view).
    /// </summary>
    [Fact]
    public async Task Get_All_Avaliacoes_Should_Return_List_Or_NoContent_When_Authenticated()
    {
        await AuthenticateAsAdminAsync();
        var response = await _client.GetAsync(BaseUrl);
        
        // Pode ser 200 (com dados) ou 204 (vazio)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Testa obter avaliação por ID que existe.
    /// </summary>
    [Fact]
    public async Task Get_Avaliacao_By_Id_Should_Return_Data_When_Exists()
    {
        await AuthenticateAsAdminAsync();
        
        // Setup: Criar Produto -> Criar Avaliação -> Obter ID
        var product = ProductTestDataBuilder.CreateValidProduct(name: $"Prod GetById {Guid.NewGuid()}");
        await _client.PostAsJsonAsync(ProductsUrl, product);
        
        // Busca produto para pegar ID (via listagem pois Create não retorna)
        Guid? produtoId = null;
        var listResponse = await _client.GetAsync(ProductsUrl);
        var listContent = await listResponse.Content.ReadAsStringAsync();
        using (var doc = JsonDocument.Parse(listContent))
        {
            var dataElement = doc.RootElement.GetProperty("data");
            JsonElement productsList = dataElement;
            if (dataElement.ValueKind == JsonValueKind.Object && dataElement.TryGetProperty("products", out var inner))
                productsList = inner;

            foreach (var item in productsList.EnumerateArray())
            {
                if (item.GetProperty("name").GetString() == product.Name)
                {
                    produtoId = item.GetProperty("id").GetGuid();
                    break;
                }
            }
        }
        
        // Criar avaliação
        var avaliacao = AvaliacaoTestDataBuilder.CreateValidAvaliacao(produtoId!.Value, 5, "Teste GetById");
        await _client.PostAsJsonAsync(BaseUrl, avaliacao);
        
        // Busca avaliação por produto para pegar ID
        var ratingsResponse = await _client.GetAsync($"{BaseUrl}/products/{produtoId}");
        var ratingsContent = await ratingsResponse.Content.ReadAsStringAsync();
        Guid? avaliacaoId = null;
        using (var doc = JsonDocument.Parse(ratingsContent))
        {
            var dataElement = doc.RootElement.GetProperty("data");
            JsonElement list = dataElement;
            if (dataElement.ValueKind == JsonValueKind.Object && dataElement.TryGetProperty("reviews", out var inner))
                list = inner;
                
            foreach (var item in list.EnumerateArray())
            {
                if (item.TryGetProperty("comment", out var c) && c.GetString() == "Teste GetById")
                {
                    avaliacaoId = item.GetProperty("id").GetGuid();
                    break;
                }
            }
        }

        // Teste propriamente dito
        var response = await _client.GetAsync($"{BaseUrl}/{avaliacaoId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Teste GetById");
    }

    /// <summary>
    /// Testa obter avaliação por ID inexistente.
    /// </summary>
    [Fact]
    public async Task Get_Avaliacao_By_Id_Should_Return_Error_When_Not_Exists()
    {
        await AuthenticateAsAdminAsync();
        var randomId = Guid.NewGuid();
        var response = await _client.GetAsync($"{BaseUrl}/{randomId}");
        
        // Padrão da API parece ser 204 (No Content) para Null em alguns casos, ou 404
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Testa validação de dados inválidos na criação (Nota fora do range).
    /// </summary>
    [Fact]
    public async Task Create_Avaliacao_Invalid_Rating_Should_Return_400()
    {
        await AuthenticateAsAdminAsync();
        // Nota 6 (máximo é 5)
        var avaliacao = AvaliacaoTestDataBuilder.CreateValidAvaliacao(Guid.NewGuid(), 6);
        
        var response = await _client.PostAsJsonAsync(BaseUrl, avaliacao);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Testa obter avaliações por Usuario ID (endpoint específico do controller).
    /// </summary>
    [Fact]
    public async Task Get_Avaliacoes_By_UserId_Should_Return_200_Or_204()
    {
        await AuthenticateAsAdminAsync(); 
        // Como não sabemos facilmente o ID do admin logado ou se ele tem reviews,
        // testamos com um GUID aleatório, esperando Lista Vazia (200 [] ou 204).
        // Se a API validar que "usuário não existe", pode dar 404.
        
        var randomUserId = Guid.NewGuid();
        var response = await _client.GetAsync($"{BaseUrl}/users/{randomUserId}");
        
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}
