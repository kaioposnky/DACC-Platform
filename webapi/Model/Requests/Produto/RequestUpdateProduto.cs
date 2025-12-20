using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para atualizar um produto.
    /// </summary>
    public class RequestUpdateProduto
    {
        /// <summary>
        /// Obtém ou define o novo nome do produto.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Obtém ou define a nova descrição do produto.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Obtém ou define a nova categoria do produto.
        /// </summary>
        public string? Categoria { get; set; }

        /// <summary>
        /// Obtém ou define a nova subcategoria do produto.
        /// </summary>
        public string? Subcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o novo preço do produto.
        /// </summary>
        public double? Preco { get; set; }
        
        /// <summary>
        /// Obtém ou define o novo preço original do produto.
        /// </summary>
        public double? PrecoOriginal { get; set; }
        
        public string? DescricaoDetalhada { get; set; }
        
        public List<string>? PerfeitoPara { get; set; }
        
        public bool? Destaque { get; set; }
        
        public List<SpecificationItem>? Especificacoes { get; set; }
        
        public ShippingInfo? InformacaoEnvio { get; set; }
    }
}
