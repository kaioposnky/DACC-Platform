using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Products;

public class ProdutosControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Testa a criação de um produto com todos os campos válidos, autenticado como administrador.
    /// </summary>
    [Fact]
    public async Task Create_Product_Should_Return_201_When_Authenticated_As_Admin()
    {
        await AuthenticateAsAdminAsync();
        var categoriaId = await GetCategoriaIdAsync("roupas");
        var subcategoriaId = await GetSubcategoriaIdAsync("camisetas");
        
        var product = ProductTestDataBuilder.CreateValidProduct(
            categoria: categoriaId,
            subcategoria: subcategoriaId
        );

        var response = await _client.PostAsJsonAsync("v1/api/products", product);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar produto: {response.StatusCode} - {errorContent}");
        }
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa a criação de um produto enviando apenas os campos obrigatórios.
    /// </summary>
    [Fact]
    public async Task Create_Product_With_Minimal_Data_Should_Return_201_When_Authenticated()
    {
        await AuthenticateAsAdminAsync();
        var categoriaId = await GetCategoriaIdAsync("outros");
        var subcategoriaId = await GetSubcategoriaIdAsync("adesivos");
        
        var product = ProductTestDataBuilder.CreateMinimalProduct(
            categoria: categoriaId,
            subcategoria: subcategoriaId
        );

        var response = await _client.PostAsJsonAsync("v1/api/products", product);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Testa se a API bloqueia a criação de produto sem autenticação.
    /// </summary>
    [Fact]
    public async Task Create_Product_Without_Auth_Should_Return_401()
    {
        var product = ProductTestDataBuilder.CreateValidProduct();

        var response = await _client.PostAsJsonAsync("v1/api/products", product);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Testa se a API valida campos obrigatórios ou inválidos (ex: nome muito curto).
    /// </summary>
    [Fact]
    public async Task Create_Product_With_Invalid_Name_Should_Return_400()
    {
        await AuthenticateAsAdminAsync();
        var product = ProductTestDataBuilder.CreateInvalidProduct("nome_curto");

        var response = await _client.PostAsJsonAsync("v1/api/products", product);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Testa a listagem de produtos, aceitando 200 (se houver) ou 404 (se vazio).
    /// </summary>
    [Fact]
    public async Task Get_Products_Should_Return_200_Or_404()
    {
        var response = await _client.GetAsync("v1/api/products");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Testa a obtenção de um produto específico pelo ID após criá-lo.
    /// </summary>
    [Fact]
    public async Task Get_Product_By_Id_Should_Return_200_When_Exists()
    {
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        var productRequest = ProductTestDataBuilder.CreateValidProduct(categoria: catId, subcategoria: subId);
        
        var createResponse = await _client.PostAsJsonAsync("v1/api/products", productRequest);
        var createData = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productId = createData!.Data;

        var response = await _client.GetAsync($"v1/api/products/{productId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Testa a obtenção de um produto que não existe, esperando 404.
    /// </summary>
    [Fact]
    public async Task Get_Product_By_Id_Should_Return_404_When_Not_Exists()
    {
        var randomId = Guid.NewGuid();

        var response = await _client.GetAsync($"v1/api/products/{randomId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Testa a busca de produtos por critérios (ex: parte do nome).
    /// </summary>
    [Fact]
    public async Task Search_Products_Should_Return_200_When_CriteriaMatches()
    {
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        var productRequest = ProductTestDataBuilder.CreateValidProduct(nome: "BuscaTeste", categoria: catId, subcategoria: subId);
        await _client.PostAsJsonAsync("v1/api/products", productRequest);

        var response = await _client.GetAsync("v1/api/products/search?searchQuery=BuscaTeste");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Testa a remoção de um produto pelo administrador.
    /// </summary>
    [Fact]
    public async Task Remove_Product_Should_Return_200_When_Authenticated_As_Admin()
    {
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        var productRequest = ProductTestDataBuilder.CreateValidProduct(categoria: catId, subcategoria: subId);
        
        var createResponse = await _client.PostAsJsonAsync("v1/api/products", productRequest);
        var createData = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productId = createData!.Data;

        var response = await _client.DeleteAsync($"v1/api/products/{productId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Testa se um usuário sem permissão de admin (cargo aluno) é impedido de remover produtos.
    /// </summary>
    [Fact]
    public async Task Remove_Product_Should_Return_403_When_Authenticated_As_User()
    {
        // Criação inicial como admin
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        var createResponse = await _client.PostAsJsonAsync("v1/api/products", ProductTestDataBuilder.CreateValidProduct(categoria: catId, subcategoria: subId));
        var createData = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productId = createData!.Data;

        // Troca para usuário normal
        await AuthenticateAsUserAsync();
        var response = await _client.DeleteAsync($"v1/api/products/{productId}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Testa a atualização de um produto pelo administrador.
    /// </summary>
    [Fact]
    public async Task Update_Product_Should_Return_200_When_Authenticated_As_Admin()
    {
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        
        // 1. Cria o produto
        var createResponse = await _client.PostAsJsonAsync("v1/api/products", ProductTestDataBuilder.CreateValidProduct(categoria: catId, subcategoria: subId));
        var createData = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        var productId = createData!.Data;

        // 2. Atualiza o produto (PATCH + FormData)
        var updateRequest = ProductTestDataBuilder.CreateUpdateProduct(nome: "Produto Totalmente Novo", preco: 150.00);
        var formData = ToFormData(updateRequest);
        
        var response = await _client.PatchAsync($"v1/api/products/{productId}", formData);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao atualizar produto: {response.StatusCode} - {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // 3. Verifica se atualizou mesmo
        var getResponse = await _client.GetAsync($"v1/api/products/{productId}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        getContent.Should().Contain("Produto Totalmente Novo");
        getContent.Should().Contain("150");
    }

    /// <summary>
    /// Testa a criação, atualização e remoção de uma variação de produto.
    /// </summary>
    [Fact]
    public async Task Variation_Lifecycle_Should_Work_Correctly()
    {
        await AuthenticateAsAdminAsync();
        var catId = await GetCategoriaIdAsync("roupas");
        var subId = await GetSubcategoriaIdAsync("camisetas");
        
        // 1. Cria o produto base
        var productRequest = ProductTestDataBuilder.CreateValidProduct(categoria: catId, subcategoria: subId);
        var createProdResp = await _client.PostAsJsonAsync("v1/api/products", productRequest);
        var productId = (await createProdResp.Content.ReadFromJsonAsync<CreateProductResponse>())!.Data;

        // 2. Cria uma variação (POST + FormData + URL Correta)
        var variationRequest = ProductTestDataBuilder.CreateVariationRequest(cor: "Verde", tamanho: "P", estoque: 20);
        var createFormData = ToFormData(variationRequest);
        
        var createVarResp = await _client.PostAsync($"v1/api/products/{productId}/variations", createFormData);
        
        if (!createVarResp.IsSuccessStatusCode)
        {
            var errorContent = await createVarResp.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar variação: {createVarResp.StatusCode} - {errorContent}");
        }
        
        createVarResp.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // A API retorna o objeto completo da variação em Data, pegamos o ID de lá.
        var variationData = await createVarResp.Content.ReadFromJsonAsync<GenericResponse<VariationResponse>>();
        var variationId = variationData!.Data.Id;

        // 3. Atualiza a variação (PATCH + FormData + URL Correta com ProductId)
        var updateVarRequest = ProductTestDataBuilder.CreateUpdateVariation(estoque: 100);
        var updateFormData = ToFormData(updateVarRequest);
        
        var updateVarResp = await _client.PatchAsync($"v1/api/products/{productId}/variations/{variationId}", updateFormData);
        
        if (!updateVarResp.IsSuccessStatusCode)
        {
            var errorContent = await updateVarResp.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao atualizar variação: {updateVarResp.StatusCode} - {errorContent}");
        }
        
        updateVarResp.StatusCode.Should().Be(HttpStatusCode.OK);

        // 4. Verifica se a atualização refletiu no GET do produto
        var getProdResp = await _client.GetAsync($"v1/api/products/{productId}");
        var prodContent = await getProdResp.Content.ReadAsStringAsync();
        prodContent.Should().Contain("100"); // Estoque atualizado

        // 5. Remove a variação (DELETE + URL Correta com ProductId)
        var deleteVarResp = await _client.DeleteAsync($"v1/api/products/{productId}/variations/{variationId}");
        deleteVarResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    private MultipartFormDataContent ToFormData<T>(T data)
    {
        var formData = new MultipartFormDataContent();
        var properties = typeof(T).GetProperties();
        
        foreach (var prop in properties)
        {
            var value = prop.GetValue(data);
            if (value != null)
            {
                formData.Add(new StringContent(value.ToString()!), prop.Name);
            }
        }
        return formData;
    }
    
    // Classes auxiliares para deserialização. 
    private class CreateProductResponse { public Guid Data { get; set; } }
    private class GenericResponse<T> { public T Data { get; set; } = default!; }
    private class VariationResponse { public Guid Id { get; set; } }
}
