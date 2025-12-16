using DaccApi.Enum.Produtos;
using DaccApi.Helpers;
using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um produto no sistema.
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// Obtém ou define o ID do produto.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Obtém ou define o preço atual do produto.
        /// </summary>
        public double? Preco { get; set; }
        /// <summary>
        /// Obtém ou define o preço original do produto (para promoções).
        /// </summary>
        public double? PrecoOriginal { get; set; }
        /// <summary>
        /// Obtém ou define a categoria do produto.
        /// </summary>
        public string Categoria { get; set; }
        /// <summary>
        /// Obtém ou define a subcategoria do produto.
        /// </summary>
        public string Subcategoria { get; set; }
        /// <summary>
        /// Obtém ou define se o produto está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;
        /// <summary>
        /// Obtém ou define a data de criação do produto.
        /// </summary>
        public DateTime? DataCriacao { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização do produto.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
        
        /// <summary>
        /// Obtém ou define a lista de variações do produto.
        /// </summary>
        public List<ProdutoVariacao> Variacoes { get; set; } = new();
        
        /// <summary>
        /// Mapeia uma variação de produto para seu objeto de resposta.
        /// </summary>
        public static ResponseProdutoVariacao MapToResponseVariacao(ProdutoVariacao variation)
        {
            return new ResponseProdutoVariacao(variation);
            return new ResponseProdutoVariacao
            {
                Id = variation.Id,
                ProdutoId = variation.ProdutoId,
                Cor = variation.Cor,
                Tamanho = variation.Tamanho,
                Estoque = variation.Estoque,
                Sku = variation.Sku,
                Ordem = variation.Ordem,
                Imagens = variation.Imagens?.Select(img => new ResponseProdutoImagem
                {
                    Id = img.Id,
                    ProdutoVariacaoId = img.ProdutoVariacaoId,
                    ImagemUrl = img.ImagemUrl,
                    ImagemAlt = img.ImagemAlt,
                    Ordem = img.Ordem,
                }).ToList() ?? new List<ResponseProdutoImagem>(),
                DataCriacao = variation.DataCriacao,
                DataAtualizacao = variation.DataAtualizacao
            };
        }

        /// <summary>
        /// Mapeia um produto e suas variações para seu objeto de resposta.
        /// </summary>
        public static ResponseProduto MapToResponseProduto(Produto product, List<ProdutoVariacao> variations)
        {
            return new ResponseProduto
            {
                Id = product.Id,
                Nome = product.Nome,
                Descricao = product.Descricao,
                Preco = product.Preco,
                Categoria = product.Categoria,
                Subcategoria = product.Subcategoria,
                Variacoes = variations.Select(MapToResponseVariacao).ToList(),
                DataCriacao = product.DataCriacao,
                DataAtualizacao = product.DataAtualizacao
            };
        }

        /// <summary>
        /// Cria um objeto Produto a partir de uma requisição de criação.
        /// </summary>
        public static Produto FromRequest(RequestCreateProduto request, Guid productId)
        {
            return new Produto
            {
                Id = productId,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Categoria = request.Categoria,
                Subcategoria = request.Subcategoria,
                Preco = request.Preco,
                PrecoOriginal = request.Preco,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Atualiza as propriedades do produto a partir de uma requisição de atualização.
        /// </summary>
        public void UpdateFromRequest(RequestCreateProduto request)
        {
            Nome = request.Nome;
            Descricao = request.Descricao;
            Categoria = request.Categoria;
            Subcategoria = request.Subcategoria;
            Preco = request.Preco;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
