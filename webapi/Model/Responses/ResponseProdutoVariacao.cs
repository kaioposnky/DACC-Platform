namespace DaccApi.Model.Responses
{
    using System.Linq;

    /// <summary>
    /// Representa a resposta de uma variação de produto.
    /// </summary>
    public class ResponseProdutoVariacao
    {
        /// <summary>
        /// Obtém ou define o ID da variação.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define a cor da variação.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define o tamanho da variação.
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define a quantidade em estoque.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Obtém ou define o SKU da variação.
        /// </summary>
        public string Sku { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define a ordem de exibição.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Obtém ou define a lista de imagens da variação.
        /// </summary>
        public List<ResponseProdutoImagem> Images { get; set; } = new();

        /// <summary>
        /// Obtém ou define se a variação está em estoque.
        /// </summary>
        public bool InStock { get; set; }

        /// <summary>
        /// Obtém ou define a data de criação.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade ProdutoVariacao.
        /// </summary>
        /// <param name="produtoVariacao">A entidade ProdutoVariacao de origem.</param>
        public ResponseProdutoVariacao(ProdutoVariacao produtoVariacao)
        {
            Id = produtoVariacao.Id;
            Color = produtoVariacao.Cor;
            Size = produtoVariacao.Tamanho;
            Stock = produtoVariacao.Estoque;
            Sku = produtoVariacao.Sku;
            Order = produtoVariacao.Ordem;
            InStock = produtoVariacao.EmEstoque; // Derived property from ProdutoVariacao
            CreatedAt = produtoVariacao.DataCriacao;
            UpdatedAt = produtoVariacao.DataAtualizacao;

            Images = produtoVariacao.Imagens?.Select(img => new ResponseProdutoImagem(img)).ToList() ??
                     new List<ResponseProdutoImagem>();
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponseProdutoVariacao()
        {
        }
    }
}