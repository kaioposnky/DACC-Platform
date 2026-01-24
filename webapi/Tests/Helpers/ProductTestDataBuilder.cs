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
        string? nome = null,
        string? descricao = null,
        string? categoria = null,
        string? subcategoria = null,
        double? preco = null,
        bool destaque = false)
    {
        return new RequestCreateProduto
        {
            Nome = nome ?? "Camiseta DACC Premium",
            Descricao = descricao ?? "Camiseta de alta qualidade com logo do DACC, perfeita para o dia a dia",
            Categoria = categoria ?? DefaultCategoriaId,
            Subcategoria = subcategoria ?? DefaultSubcategoriaId,
            Preco = preco ?? 79.90,
            DescricaoDetalhada = "Camiseta 100% algodão, com estampa de alta durabilidade. " +
                                 "Ideal para estudantes e profissionais de tecnologia.",
            PerfeitoPara = new List<string> { "Uso diário", "Eventos acadêmicos", "Hackathons" },
            Destaque = destaque,
            Especificacoes = new List<SpecificationItem>
            {
                new() { Name = "Material", Value = "100% Algodão" },
                new() { Name = "Lavagem", Value = "Máquina até 40°C" },
                new() { Name = "Origem", Value = "Nacional" }
            },
            InformacaoEnvio = new ShippingInfo
            {
                FreeShipping = true,
                EstimatedDays = "5-7 dias úteis",
                ShippingCost = 0,
                ReturnPolicy = "30 dias para devolução",
                Warranty = "Garantia de 90 dias contra defeitos de fabricação"
            }
        };
    }

    /// <summary>
    /// Cria um produto com dados mínimos obrigatórios
    /// </summary>
    public static RequestCreateProduto CreateMinimalProduct(
        string? categoria = null,
        string? subcategoria = null)
    {
        return new RequestCreateProduto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição mínima do produto de teste",
            Categoria = categoria ?? "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a12", // outros
            Subcategoria = subcategoria ?? "b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a25", // adesivos
            Preco = 9.90
        };
    }

    /// <summary>
    /// Cria um produto com dados inválidos para testes de validação
    /// </summary>
    public static RequestCreateProduto CreateInvalidProduct(string invalidField)
    {
        return invalidField switch
        {
            "nome_curto" => CreateValidProduct(nome: "AB"), // Menos de 3 caracteres
            "nome_longo" => CreateValidProduct(nome: new string('A', 51)), // Mais de 50 caracteres
            "descricao_curta" => CreateValidProduct(descricao: "Curta"), // Menos de 10 caracteres
            "descricao_longa" => CreateValidProduct(descricao: new string('A', 1001)), // Mais de 1000 caracteres
            "preco_zero" => CreateValidProduct(preco: 0), // Preço inválido
            "preco_negativo" => CreateValidProduct(preco: -10), // Preço negativo
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
                nome: $"Produto Teste {i}",
                descricao: $"Descrição do produto de teste número {i}",
                preco: 10.00 * i,
                destaque: i % 2 == 0 // Produtos pares são destaque
            ));
        }

        return products;
    }

    /// <summary>
    /// Cria um objeto RequestUpdateProduto com novos dados
    /// </summary>
    public static RequestUpdateProduto CreateUpdateProduct(
        string? nome = null,
        double? preco = null)
    {
        return new RequestUpdateProduto
        {
            Nome = nome ?? "Produto Atualizado",
            Preco = preco ?? 99.99,
            Descricao = "Nova descrição do produto atualizado"
        };
    }

    /// <summary>
    /// Cria um objeto RequestUpdateProdutoVariacao com novos dados
    /// </summary>
    public static RequestUpdateProdutoVariacao CreateUpdateVariation(
        int? estoque = null)
    {
        return new RequestUpdateProdutoVariacao
        {
            Estoque = estoque ?? 50,
            Cor = "Azul",
            Tamanho = "G",
            OrdemVariacao = 1
        };
    }

    /// <summary>
    /// Cria um objeto RequestProdutoVariacaoCreate para criar uma nova variação
    /// </summary>
    public static RequestProdutoVariacaoCreate CreateVariationRequest(
        string? cor = null,
        string? tamanho = null,
        int estoque = 10)
    {
        return new RequestProdutoVariacaoCreate
        {
            Cor = cor ?? "Preto",
            Tamanho = tamanho ?? "M",
            Estoque = estoque,
            OrdemVariacao = 0
        };
    }
}
