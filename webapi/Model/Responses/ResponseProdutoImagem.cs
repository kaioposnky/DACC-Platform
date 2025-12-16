namespace DaccApi.Model.Responses
{
    /// <summary>
    /// Representa a resposta de uma imagem de produto.
    /// </summary>
    public class ResponseProdutoImagem
    {
        /// <summary>
        /// Obtém ou define o ID da imagem.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem.
        /// </summary>
        public string? Url { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? Alt { get; set; }
        /// <summary>
        /// Obtém ou define a ordem de exibição da imagem.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade ProdutoImagem.
        /// </summary>
        /// <param name="produtoImagem">A entidade ProdutoImagem de origem.</param>
        public ResponseProdutoImagem(ProdutoImagem produtoImagem)
        {
            Id = produtoImagem.Id;
            Url = produtoImagem.ImagemUrl;
            Alt = produtoImagem.ImagemAlt;
            Order = produtoImagem.Ordem;
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponseProdutoImagem()
        {
        }
    }
}
