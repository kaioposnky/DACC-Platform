using DaccApi.Model;
using DaccApi.Model.Responses;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de produtos
/// </summary>
public static class ProductTestDataBuilder
{
    // IDs fixos baseados no seed do sqlcode.sql
    // Categoria "roupas" e subcategoria "camisetas"
    private const string DefaultCategoriaId = "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"; // roupas
    private const string DefaultSubcategoriaId = "b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a22"; // camisetas

    /// <summary>
    /// Cria um objeto RequestCreateProduto válido com dados padrão
    /// </summary>
    public static RequestCreateProduto CreateValidProduct(
        string? name = null,
        string? description = null,
        string? category = null,
        string? subcategory = null,
        double? price = null,
        bool featured = false)
    {
        return new RequestCreateProduto
        {
            Name = name ?? "Camiseta DACC Premium",
            Description = description ?? "Camiseta de alta qualidade com logo do DACC, perfeita para o dia a dia",
            Category = category ?? DefaultCategoriaId,
            // Subcategory field is disabled in RequestCreateProduto
            // Subcategory = subcategory ?? DefaultSubcategoriaId,
            Price = price ?? 79.90,
            DetailedDescription = "Camiseta 100% algodão, com estampa de alta durabilidade. " +
                                 "Ideal para estudantes e profissionais de tecnologia.",
            PerfectFor = new List<string> { "Uso diário", "Eventos acadêmicos", "Hackathons" },
            Featured = featured,
            Specifications = new List<SpecificationItem>
            {
                new() { Name = "Material", Value = "100% Algodão" },
                new() { Name = "Lavagem", Value = "Máquina até 40°C" },
                new() { Name = "Origem", Value = "Nacional" }
            }
            // ShippingInfo field is disabled in RequestCreateProduto
            // ShippingInfo = new ShippingInfo
            // {
            //     FreeShipping = true,
            //     EstimatedDays = 0,
            //     ShippingCost = 0,
            //     ReturnPolicy = "30 dias para devolução",
            //     Warranty = "Garantia de 90 dias contra defeitos de fabricação"
            // }
        };
    }

    /// <summary>
    /// Cria um produto com dados mínimos obrigatórios
    /// </summary>
    public static RequestCreateProduto CreateMinimalProduct(
        string? category = null,
        string? subcategory = null)
    {
        return new RequestCreateProduto
        {
            Name = "Produto Teste",
            Description = "Descrição mínima do produto de teste",
            Category = category ?? "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a12", // outros
            // Subcategory field is disabled in RequestCreateProduto
            // Subcategory = subcategory ?? "b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a25", // adesivos
            Price = 9.90
        };
    }

    /// <summary>
    /// Cria um produto com dados inválidos para testes de validação
    /// </summary>
    public static RequestCreateProduto CreateInvalidProduct(string invalidField)
    {
        return invalidField switch
        {
            "nome_curto" => CreateValidProduct(name: "AB"), // Menos de 3 caracteres
            "nome_longo" => CreateValidProduct(name: new string('A', 51)), // Mais de 50 caracteres
            "descricao_curta" => CreateValidProduct(description: "Curta"), // Menos de 10 caracteres
            "descricao_longa" => CreateValidProduct(description: new string('A', 1001)), // Mais de 1000 caracteres
            "preco_zero" => CreateValidProduct(price: 0), // Preço inválido
            "preco_negativo" => CreateValidProduct(price: -10), // Preço negativo
            _ => CreateValidProduct()
        };
    }

    /// <summary>
    /// Cria múltiplos produtos para testes em lote
    /// </summary>
    public static List<RequestCreateProduto> CreateMultipleProducts(int count)
    {
        var products = new List<RequestCreateProduto>();

        for (int i = 1; i <= count; i++)
        {
            products.Add(CreateValidProduct(
                name: $"Produto Teste {i}",
                description: $"Descrição do produto de teste número {i}",
                price: 10.00 * i,
                featured: i % 2 == 0 // Produtos pares são destaque
            ));
        }

        return products;
    }

    /// <summary>
    /// Cria um objeto RequestUpdateProduto com novos dados
    /// </summary>
    public static RequestUpdateProduto CreateUpdateProduct(
        string? name = null,
        double? price = null)
    {
        return new RequestUpdateProduto
        {
            Name = name ?? "Produto Atualizado",
            Price = price ?? 99.99,
            Description = "Nova descrição do produto atualizado"
        };
    }

    /// <summary>
    /// Cria um objeto RequestUpdateProdutoVariacao com novos dados
    /// </summary>
    public static RequestUpdateProdutoVariacao CreateUpdateVariation(
        int? stock = null)
    {
        return new RequestUpdateProdutoVariacao
        {
            Stock = stock ?? 50,
            Color = "Azul",
            Size = "G",
            DisplayOrder = 1
        };
    }

    /// <summary>
    /// Cria um objeto RequestProdutoVariacaoCreate para criar uma nova variação
    /// </summary>
    public static RequestProdutoVariacaoCreate CreateVariationRequest(
        string? color = null,
        string? size = null,
        int stock = 10)
    {
        return new RequestProdutoVariacaoCreate
        {
            Color = color ?? "Preto",
            Size = size ?? "M",
            Stock = stock,
            DisplayOrder = 0
        };
    }
}
