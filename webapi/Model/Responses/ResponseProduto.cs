namespace DaccApi.Model.Responses;

using System.Linq;

/// <summary>
/// Representa a resposta de um produto.
/// </summary>
public class ResponseProduto
{
    /// <summary>
    /// Obtém ou define o ID do produto.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a descrição do produto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a descrição detalhada do produto (frontend specific).
    /// </summary>
    public string? DetailedDescription { get; set; }

    /// <summary>
    /// Obtém ou define para quem o produto é perfeito (frontend specific).
    /// </summary>
    public List<string>? PerfectFor { get; set; }

    /// <summary>
    /// Obtém ou define o preço do produto.
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Obtém ou define o preço original do produto (para promoções).
    /// </summary>
    public double? OriginalPrice { get; set; }

    /// <summary>
    /// Obtém ou define a categoria do produto.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define se o produto está em estoque (agregado das variações).
    /// </summary>
    public bool InStock { get; set; }

    /// <summary>
    /// Obtém ou define a contagem total de estoque do produto (agregado das variações).
    /// </summary>
    public int StockCount { get; set; }

    /// <summary>
    /// Obtém ou define a URL da imagem principal do produto.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Obtém ou define a lista de URLs de imagens do produto (agregado das variações).
    /// </summary>
    public List<string?>? Images { get; set; }

    /// <summary>
    /// Obtém ou define a lista de tamanhos disponíveis (agregado das variações).
    /// </summary>
    public List<string>? Sizes { get; set; }

    /// <summary>
    /// Obtém ou define a lista de cores disponíveis (agregado das variações).
    /// </summary>
    public List<string>? Colors { get; set; }

    /// <summary>
    /// Obtém ou define se o produto é destacado (frontend specific).
    /// </summary>
    public bool? Featured { get; set; }

    /// <summary>
    /// Obtém ou define a avaliação média do produto (frontend specific).
    /// </summary>
    public double Rating { get; set; }

    /// <summary>
    /// Obtém ou define o número de avaliações do produto (frontend specific).
    /// </summary>
    public int Reviews { get; set; }

    /// <summary>
    /// Obtém ou define a lista de avaliações do produto (frontend specific).
    /// </summary>
    public List<ResponseAvaliacaoProduto>?
        ReviewsList { get; set; } // Will be mapped if ResponseAvaliacaoProduto is created

    /// <summary>
    /// Obtém ou define as especificações do produto (frontend specific).
    /// </summary>
    public List<SpecificationItem>? Specifications { get; set; }

    /// <summary>
    /// Obtém ou define as informações de envio do produto (frontend specific).
    /// </summary>
    public ShippingInfo? ShippingInfo { get; set; }

    /// <summary>
    /// Obtém ou define a lista de variações do produto.
    /// </summary>
    public List<ResponseProdutoVariacao> Variations { get; set; } = new();

    /// <summary>
    /// Obtém ou define a data de criação do produto.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Obtém ou define a data da última atualização do produto.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Produto.
    /// </summary>
    /// <param name="produto">A entidade Produto de origem.</param>
    public ResponseProduto(Produto produto)
    {
        Id = produto.Id;
        Name = produto.Nome;
        Description = produto.Descricao;
        Price = produto.Preco;
        OriginalPrice = produto.PrecoOriginal;
        Category = !string.IsNullOrEmpty(produto.CategoriaNome) ? produto.CategoriaNome : produto.Categoria.ToString();
        CreatedAt = produto.DataCriacao;
        UpdatedAt = produto.DataAtualizacao;

        // Mapeia variações
        Variations = produto.Variacoes?.Select(v => new ResponseProdutoVariacao(v)).ToList() ??
                     new List<ResponseProdutoVariacao>();

        // Propriedades agregadas/derivadas das variações ou frontend specific
        InStock = Variations.Any(v => v.InStock);
        StockCount = Variations.Sum(v => v.Stock);
        Image = Variations.FirstOrDefault()?.Images?.FirstOrDefault()
            ?.Url; // Primary image from first variation's first image
        Images = Variations.SelectMany(v => v.Images?.Select(img => img.Url) ?? Enumerable.Empty<string>())
            .Where(url => url != null).ToList();
        Sizes = Variations.Select(v => v.Size).Distinct().ToList();
        Colors = Variations.Select(v => v.Color).Distinct().ToList();

        // Propriedades específicas do frontend sem correspondência direta na entidade Produto
        DetailedDescription = null; // Preencher via lógica adicional
        PerfectFor = null; // Preencher via lógica adicional
        Featured = false; // Valor padrão
        Rating = 0; // Valor padrão
        Reviews = 0; // Valor padrão
        ReviewsList = null; // Preencher via lógica adicional
        Specifications = null; // Preencher via lógica adicional
        ShippingInfo = null; // Preencher via lógica adicional
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseProduto()
    {
    }
}

/// <summary>
/// Representa um item de especificação do produto para o frontend.
/// </summary>
public class SpecificationItem
{
    /// <summary>
    /// Obtém ou define o nome da especificação.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Obtém ou define o valor da especificação.
    /// </summary>
    public string? Value { get; set; }
}

/// <summary>
/// Representa as informações de envio do produto para o frontend.
/// </summary>
public class ShippingInfo
{
    /// <summary>
    /// Obtém ou define se há frete grátis.
    /// </summary>
    public bool FreeShipping { get; set; }

    /// <summary>
    /// Obtém ou define os dias estimados para entrega.
    /// </summary>

    public string EstimatedDays { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o custo de envio.
    /// </summary>
    public double? ShippingCost { get; set; }

    /// <summary>
    /// Obtém ou define a política de devolução.
    /// </summary>
    public string ReturnPolicy { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a garantia.
    /// </summary>
    public string? Warranty { get; set; }
}
