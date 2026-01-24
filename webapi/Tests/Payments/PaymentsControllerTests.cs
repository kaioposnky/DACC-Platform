using System.Net;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Payments;

/// <summary>
/// Testes de integração para PaymentsController.
/// </summary>
public class PaymentsControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/payments";

    [Fact]
    public async Task Get_Success_Should_Return_200_With_Data()
    {
        var externalRef = "order_123";
        var response = await _client.GetAsync($"{BaseUrl}/success?external_reference={externalRef}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        
        // Verifica se retorna o external_reference e mensagem
        content.Should().Contain(externalRef);
        content.Should().Contain("sucesso");
    }

    [Fact]
    public async Task Get_Failure_Should_Return_200_With_Data()
    {
        var externalRef = "order_fail_123";
        var response = await _client.GetAsync($"{BaseUrl}/failure?external_reference={externalRef}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        
        content.Should().Contain(externalRef);
        content.Should().Contain("falhou");
    }

    [Fact]
    public async Task Get_Pending_Should_Return_200_With_Data()
    {
        var externalRef = "order_pending_123";
        var response = await _client.GetAsync($"{BaseUrl}/pending?external_reference={externalRef}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        
        content.Should().Contain(externalRef);
        content.Should().Contain("pendente");
    }
}
