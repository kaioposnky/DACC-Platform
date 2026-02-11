using System.Net;
using System.Net.Http.Json;
using DaccApi.Model.Responses.Produto;
using FluentAssertions;
using Xunit;

namespace DaccApi.Tests.Products
{
    /// <summary>
    /// Testes de integração para os endpoints de filtros de produtos (categorias, subcategorias, tamanhos, cores).
    /// </summary>
    public class ProductFiltersTests : IntegrationTestBase
    {
        public ProductFiltersTests() : base()
        {
        }

        #region Categories Tests

        [Fact]
        public async Task Get_Categories_Should_Return_Success()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/categories");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_Categories_Should_Return_Valid_Structure()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/categories");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<TestResponse<CategoriesData>>();
            
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Categories.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Categories_Should_Return_Ordered_By_Name()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/categories");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<CategoriesData>>();

            // Assert
            if (result?.Data?.Categories?.Count > 1)
            {
                var categories = result.Data.Categories;
                for (int i = 0; i < categories.Count - 1; i++)
                {
                    string.Compare(categories[i].Name, categories[i + 1].Name, StringComparison.Ordinal)
                        .Should().BeLessOrEqualTo(0, "categories should be ordered alphabetically");
                }
            }
        }

        #endregion

        #region Subcategories Tests

        [Fact]
        public async Task Get_Subcategories_Should_Return_Success()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/subcategories");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_Subcategories_Should_Return_Valid_Structure()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/subcategories");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<TestResponse<SubcategoriesData>>();
            
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Subcategories.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Subcategories_Should_Have_CategoryId()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/subcategories");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<SubcategoriesData>>();

            // Assert
            if (result?.Data?.Subcategories?.Count > 0)
            {
                foreach (var subcategory in result.Data.Subcategories)
                {
                    subcategory.Id.Should().NotBeEmpty("each subcategory should have an ID");
                    subcategory.Name.Should().NotBeNullOrEmpty("each subcategory should have a name");
                    subcategory.CategoryId.Should().NotBeEmpty("each subcategory should have a category ID");
                }
            }
        }

        [Fact]
        public async Task Get_Subcategories_Should_Return_Ordered_By_Name()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/subcategories");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<SubcategoriesData>>();

            // Assert
            if (result?.Data?.Subcategories?.Count > 1)
            {
                var subcategories = result.Data.Subcategories;
                for (int i = 0; i < subcategories.Count - 1; i++)
                {
                    string.Compare(subcategories[i].Name, subcategories[i + 1].Name, StringComparison.Ordinal)
                        .Should().BeLessOrEqualTo(0, "subcategories should be ordered alphabetically");
                }
            }
        }

        #endregion

        #region Sizes Tests

        [Fact]
        public async Task Get_Sizes_Should_Return_Success()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/sizes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_Sizes_Should_Return_Valid_Structure()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/sizes");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<TestResponse<SizesData>>();
            
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Sizes.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Sizes_Should_Return_Unique_Values()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/sizes");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<SizesData>>();

            // Assert
            if (result?.Data?.Sizes?.Count > 0)
            {
                var sizes = result.Data.Sizes.Select(s => s.Value).ToList();
                sizes.Should().OnlyHaveUniqueItems("sizes should be unique");
            }
        }

        [Fact]
        public async Task Get_Sizes_Should_Have_Label_And_Value()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/sizes");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<SizesData>>();

            // Assert
            if (result?.Data?.Sizes?.Count > 0)
            {
                foreach (var size in result.Data.Sizes)
                {
                    size.Label.Should().NotBeNullOrEmpty("each size should have a label");
                    size.Value.Should().NotBeNullOrEmpty("each size should have a value");
                    size.Label.Should().Be(size.Value, "label and value should match for sizes");
                }
            }
        }

        #endregion

        #region Colors Tests

        [Fact]
        public async Task Get_Colors_Should_Return_Success()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/colors");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_Colors_Should_Return_Valid_Structure()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/colors");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<TestResponse<ColorsData>>();
            
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Colors.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Colors_Should_Return_Unique_Values()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/colors");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<ColorsData>>();

            // Assert
            if (result?.Data?.Colors?.Count > 0)
            {
                var colors = result.Data.Colors.Select(c => c.Value).ToList();
                colors.Should().OnlyHaveUniqueItems("colors should be unique");
            }
        }

        [Fact]
        public async Task Get_Colors_Should_Have_Label_And_Value()
        {
            // Act
            var response = await _client.GetAsync("/v1/api/products/colors");
            var result = await response.Content.ReadFromJsonAsync<TestResponse<ColorsData>>();

            // Assert
            if (result?.Data?.Colors?.Count > 0)
            {
                foreach (var color in result.Data.Colors)
                {
                    color.Label.Should().NotBeNullOrEmpty("each color should have a label");
                    color.Value.Should().NotBeNullOrEmpty("each color should have a value");
                    color.Label.Should().Be(color.Value, "label and value should match for colors");
                }
            }
        }

        #endregion

        #region Helper Classes

        private class TestResponse<T>
        {
            public bool Success { get; set; }
            public string Code { get; set; } = string.Empty;
            public string? Message { get; set; }
            public T? Data { get; set; }
        }

        private class CategoriesData
        {
            public List<ResponseCategoria> Categories { get; set; } = new();
        }

        private class SubcategoriesData
        {
            public List<ResponseSubcategoria> Subcategories { get; set; } = new();
        }

        private class SizesData
        {
            public List<ResponseFilterOption> Sizes { get; set; } = new();
        }

        private class ColorsData
        {
            public List<ResponseFilterOption> Colors { get; set; } = new();
        }

        #endregion
    }
}
