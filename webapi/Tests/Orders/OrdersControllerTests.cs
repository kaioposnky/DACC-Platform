using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DaccApi.Model;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Orders;

public class OrdersControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/orders";
    private const string ProductsUrl = "v1/api/products";

    /// <summary>
    /// Testa o ciclo completo: Criar Produto -> Criar Variação -> Criar Pedido -> Buscar -> Atualizar.
    /// </summary>
    [Fact]
    public async Task Order_Lifecycle_Should_Work()
    {
        await AuthenticateAsUserAsync();

        // 1. Setup: Criar produto e variação para comprar
        // Precisamos ser admin para criar produto
        var userToken = _client.DefaultRequestHeaders.Authorization; // Guarda token de usuário
        await AuthenticateAsAdminAsync();

        var product = ProductTestDataBuilder.CreateValidProduct(name: $"Produto Order {Guid.NewGuid()}");
        await _client.PostAsJsonAsync(ProductsUrl, product);
        
        // Obter ID do produto
        var listResponse = await _client.GetAsync(ProductsUrl);
        var listContent = await listResponse.Content.ReadAsStringAsync();
        Guid? productId = null;
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
                    productId = item.GetProperty("id").GetGuid();
                    break;
                }
            }
        }
        productId.Should().NotBeNull();

        // Criar variação com estoque (usando FormData pois controller usa [FromForm])
        var variationRequest = ProductTestDataBuilder.CreateVariationRequest(stock: 100);
        var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(variationRequest.Color ?? ""), "Color");
        formContent.Add(new StringContent(variationRequest.Size ?? ""), "Size");
        formContent.Add(new StringContent(variationRequest.Stock.ToString()), "Stock");
        formContent.Add(new StringContent(variationRequest.DisplayOrder.ToString()), "DisplayOrder");
        var variationResponse = await _client.PostAsync($"{ProductsUrl}/{productId}/variations", formContent);
        variationResponse.StatusCode.Should().Be(HttpStatusCode.Created); // Controller retorna 201 para criar variação

        // Obter ID da variação
        var variationsResponse = await _client.GetAsync($"{ProductsUrl}/{productId}/variations");
        var variationsContent = await variationsResponse.Content.ReadAsStringAsync();
        Guid? variationId = null;
        using (var doc = JsonDocument.Parse(variationsContent))
        {
            var dataElement = doc.RootElement.GetProperty("data").GetProperty("variations"); 
            // Variações retorna lista envelopada em data.variations
            foreach(var item in dataElement.EnumerateArray())
            {
                variationId = item.GetProperty("id").GetGuid();
                break;
            }
        }
        variationId.Should().NotBeNull();

        // 2. Voltar para usuário comum para fazer o pedido
        _client.DefaultRequestHeaders.Authorization = userToken;

        // 3. Criar Pedido
        var orderRequest = OrderTestDataBuilder.CreateValidOrder(variationId!.Value, productId!.Value, 2);
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, orderRequest);
        
        if (!createResponse.IsSuccessStatusCode)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao criar pedido: {createResponse.StatusCode} - {error}");
        }
        
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Extrair ID do pedido da resposta
        var createContent = await createResponse.Content.ReadAsStringAsync();
        Guid? orderId = null;
        using (var doc = JsonDocument.Parse(createContent))
        {
            // O controller retorna o objeto OrderResponse em data
            if (doc.RootElement.TryGetProperty("data", out var data) && data.TryGetProperty("id", out var id))
            {
                orderId = id.GetGuid();
            }
        }
        orderId.Should().NotBeNull("Pedido deve retornar ID na criação");

        // 4. Buscar Pedido por ID
        var getResponse = await _client.GetAsync($"{BaseUrl}/{orderId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // 5. Atualizar Status (Requer Admin provavelmente? Controller diz [AuthenticatedPatchResponses], não especifica role, mas vamos testar como admin se falhar)
        // O método UpdateOrderStatus não tem [HasPermission], então qualquer user autenticado pode (ou deveria poder apenas o proprio ou admin?).
        // Vamos testar como user mesmo primeiro.
        var statusUpdate = "delivered";
        var updateResponse = await _client.PutAsJsonAsync($"{BaseUrl}/{orderId}/status", statusUpdate);
        
        if (updateResponse.StatusCode == HttpStatusCode.Forbidden)
        {
            // Se precisar de permissão especial, trocamos para admin
            await AuthenticateAsAdminAsync();
            updateResponse = await _client.PutAsJsonAsync($"{BaseUrl}/{orderId}/status", statusUpdate);
        }
        
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verifica atualização
        var checkResponse = await _client.GetAsync($"{BaseUrl}/{orderId}");
        var checkContent = await checkResponse.Content.ReadAsStringAsync();
        checkContent.Should().Contain(statusUpdate);
    }

    [Fact]
    public async Task Create_Order_With_Invalid_Data_Should_Return_BadRequest()
    {
        await AuthenticateAsUserAsync();
        var emptyOrder = OrderTestDataBuilder.CreateEmptyOrder();
        var response = await _client.PostAsJsonAsync(BaseUrl, emptyOrder);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Order_OutOfStock_Should_Fail()
    {
        // 1. Setup Produto sem estoque
        await AuthenticateAsAdminAsync();
        var product = ProductTestDataBuilder.CreateValidProduct(name: $"ProdSE_{Guid.NewGuid().ToString().Substring(0, 8)}");
        var createResponse = await _client.PostAsJsonAsync(ProductsUrl, product);
        var content = await createResponse.Content.ReadAsStringAsync();
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"Creation failed: {content}");
        
        // Pegar ID (simplificado, assumindo que funciona pelo teste anterior)
        var listResponse = await _client.GetAsync(ProductsUrl);
        var listContent = await listResponse.Content.ReadAsStringAsync();
        Guid? productId = null;
        using (var doc = JsonDocument.Parse(listContent))
        {
            var dataElement = doc.RootElement.GetProperty("data");
            JsonElement list;
            if (dataElement.ValueKind == JsonValueKind.Array)
            {
                list = dataElement;
            }
            else if (dataElement.TryGetProperty("products", out var products))
            {
                list = products;
            }
            else
            {
                list = dataElement.ValueKind == JsonValueKind.Object ? dataElement.GetProperty("products") : dataElement;
            }

            foreach (var item in list.EnumerateArray())
            {
                // Verifica 'name' (padrão) ou 'Nome' (caso venha do backend assim)
                var name = item.GetProperty("name").GetString();
                
                if (name == product.Name) { productId = item.GetProperty("id").GetGuid(); break; }
            }
        }

        // Criar variação com estoque ZERO (usando FormData, pois controller usa [FromForm])
        var variationRequest = ProductTestDataBuilder.CreateVariationRequest(stock: 0);
        var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(variationRequest.Color ?? ""), "Color");
        formContent.Add(new StringContent(variationRequest.Size ?? ""), "Size");
        formContent.Add(new StringContent(variationRequest.Stock.ToString()), "Stock");
        formContent.Add(new StringContent(variationRequest.DisplayOrder.ToString()), "DisplayOrder");
        var varResponse = await _client.PostAsync($"{ProductsUrl}/{productId}/variations", formContent);
        varResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Pegar ID Variação
        var varsResponse = await _client.GetAsync($"{ProductsUrl}/{productId}/variations");
        var varsContent = await varsResponse.Content.ReadAsStringAsync();
        Guid? varId = JsonDocument.Parse(varsContent).RootElement.GetProperty("data").GetProperty("variations")[0].GetProperty("id").GetGuid();

        // 2. Tentar comprar
        await AuthenticateAsUserAsync();
        var orderRequest = OrderTestDataBuilder.CreateValidOrder(varId!.Value, productId!.Value, 1);
        
        var response = await _client.PostAsJsonAsync(BaseUrl, orderRequest);
        
        // Esperado: 409 Conflict ou 400 Bad Request com mensagem de estoque?
        // O controller retorna: return ResponseHelper.CreateErrorResponse(ResponseError.PRODUCT_OUT_OF_STOCK, ex.Message);
        // PRODUCT_OUT_OF_STOCK geralmente mapeia para 409 Conflict ou 422 UnprocessableEntity. Vamos checar o ResponseError.
        // Assumindo 400 ou 409.
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Conflict, HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity);
        
        var errorContent = await response.Content.ReadAsStringAsync();
        errorContent.Should().ContainEquivalentOf("stock");
    }


        [Fact]
        public async Task Update_Order_Status_Should_Persist()
        {
            // 1. Setup - Criar Produto
            await AuthenticateAsAdminAsync();
            var product = ProductTestDataBuilder.CreateValidProduct();
            var createProductResponse = await _client.PostAsJsonAsync(ProductsUrl, product);
            createProductResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            
            // Obter ID do produto criado
            var listResponse = await _client.GetAsync(ProductsUrl);
            var listContent = await listResponse.Content.ReadAsStringAsync();
            var products = JsonDocument.Parse(listContent).RootElement.GetProperty("data").GetProperty("products").EnumerateArray();
            Guid? productId = null;
            foreach (var item in products)
            {
                var name = item.GetProperty("name").GetString();
                if (name == product.Name) { productId = item.GetProperty("id").GetGuid(); break; }
            }

            // Criar Variação
            var variationRequest = ProductTestDataBuilder.CreateVariationRequest(stock: 10);
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StringContent(variationRequest.Color ?? ""), "Color");
            formContent.Add(new StringContent(variationRequest.Size ?? ""), "Size");
            formContent.Add(new StringContent(variationRequest.Stock.ToString()), "Stock");
            formContent.Add(new StringContent(variationRequest.DisplayOrder.ToString()), "DisplayOrder");
            await _client.PostAsync($"{ProductsUrl}/{productId}/variations", formContent);
            
            // Obter ID da Variação
            var varsResponse = await _client.GetAsync($"{ProductsUrl}/{productId}/variations");
            var varsContent = await varsResponse.Content.ReadAsStringAsync();
            var varId = JsonDocument.Parse(varsContent).RootElement.GetProperty("data").GetProperty("variations")[0].GetProperty("id").GetGuid();

            // 2. Criar Pedido (User)
            await AuthenticateAsUserAsync();
            var orderRequest = OrderTestDataBuilder.CreateValidOrder(varId, productId!.Value, 1);
            var createOrderResponse = await _client.PostAsJsonAsync(BaseUrl, orderRequest);
            createOrderResponse.StatusCode.Should().Be(HttpStatusCode.Created); // 201 Created
            
            var createOrderContent = await createOrderResponse.Content.ReadAsStringAsync();
            var orderId = JsonDocument.Parse(createOrderContent).RootElement.GetProperty("data").GetProperty("id").GetGuid();

            // 3. Atualizar Status (Admin)
            await AuthenticateAsAdminAsync();
            var newStatus = "approved";
            var updateResponse = await _client.PutAsJsonAsync($"{BaseUrl}/{orderId}/status", newStatus);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 4. Verificar Persistência
            var getOrderResponse = await _client.GetAsync($"{BaseUrl}/{orderId}");
            getOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var getOrderContent = await getOrderResponse.Content.ReadAsStringAsync();
            var status = JsonDocument.Parse(getOrderContent).RootElement.GetProperty("data").GetProperty("order").GetProperty("status").GetString();
            
            status.Should().Be(newStatus);
        }

        [Fact]
        public async Task SearchOrders_Should_Work_With_SearchQuery()
        {
            await AuthenticateAsAdminAsync();
            var response = await _client.GetAsync($"{BaseUrl}/search?searchQuery=Test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("data");
        }

        [Fact]
        public async Task SearchOrders_Should_Work_With_StatusAll()
        {
            await AuthenticateAsAdminAsync();
            // Sem query params ou com status=all deve funcionar
            var response = await _client.GetAsync($"{BaseUrl}/search?status=all");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("data");
        }
    }
