using System.Net;
using System.Net.Http.Headers;
using DaccApi.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.FileStorage;

public class FileStorageControllerTests : IntegrationTestBase
{
    private const string BaseUrl = "v1/api/FileStorage";

    [Fact]
    public async Task Upload_Image_Should_Return_200_When_Authenticated()
    {
        await AuthenticateAsAdminAsync(); // Requer permissão de upload (depende do HasPermission, assumindo admin tem ou user tem)
        // O controller diz [HasPermission(AppPermissions.FileStorage.UploadImage)]. Vamos assumir que admin tem.
        
        using var content = new MultipartFormDataContent();
        
        // Mock image
        var fileContent = new ByteArrayContent(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }); // Valid JPEG header mock
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "file", "test.jpg");

        var response = await _client.PostAsync($"{BaseUrl}/uploadImage", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Upload falhou: {response.StatusCode} - {error}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Contain("url");
    }

    [Fact]
    public async Task Upload_No_File_Should_Return_BadRequest()
    {
        await AuthenticateAsAdminAsync();
        
        using var content = new MultipartFormDataContent();
        // Não adiciona arquivo
        
        var response = await _client.PostAsync($"{BaseUrl}/uploadImage", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Upload_Without_Auth_Should_Return_401()
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
        content.Add(fileContent, "file", "test.jpg");

        var response = await _client.PostAsync($"{BaseUrl}/uploadImage", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
