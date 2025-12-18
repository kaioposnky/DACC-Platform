using System.ComponentModel.DataAnnotations.Schema;
using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um produto no sistema.
    /// </summary>
    [Table("produto")]
    public class Produto
    {
        /// <summary>
        /// Obtém ou define o ID do produto.
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        [Column("descricao")]
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define o preço atual do produto.
        /// </summary>
        [Column("preco")]
        public double Preco { get; set; }

        /// <summary>
        /// Obtém ou define o preço original do produto (para promoções).
        /// </summary>
        [Column("preco_original")]
        public double? PrecoOriginal { get; set; }

        /// <summary>
        /// Obtém ou define a categoria do produto.
        /// </summary>
        [NotMapped]
        public Guid Categoria { get; set; }

        /// <summary>
        /// Obtém ou define o nome da categoria (para exibição).
        /// </summary>
        [NotMapped]
        public string CategoriaNome { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria do produto.
        /// </summary>
        [Column("subcategoria_id")]
        public Guid? Subcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o nome da subcategoria (para exibição).
        /// </summary>
        [NotMapped]
        public string SubcategoriaNome { get; set; }

        /// <summary>
        /// Obtém ou define se o produto está ativo.
        /// </summary>
        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Obtém ou define a data de criação do produto.
        /// </summary>
        [Column("data_criacao")]
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Obtém ou define a data da última atualização do produto.
        /// </summary>
        [Column("data_atualizacao")]
        public DateTime? DataAtualizacao { get; set; }
        
        /// <summary>
        /// Obtém ou define a lista de variações do produto.
        /// </summary>
        [NotMapped]
        public List<ProdutoVariacao> Variacoes { get; set; } = new();
        
        /// <summary>
        /// Mapeia uma variação de produto para seu objeto de resposta.
        /// </summary>
        public static ResponseProdutoVariacao MapToResponseVariacao(ProdutoVariacao variation)
        {
            return new ResponseProdutoVariacao(variation);
        }

        /// <summary>
        /// Mapeia um produto e suas variações para seu objeto de resposta.
        /// </summary>
        public static ResponseProduto MapToResponseProduto(Produto product, List<ProdutoVariacao> variations)
        {
            product.Variacoes = variations;
            return new ResponseProduto(product);
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
                Categoria = Guid.Parse(request.Categoria),
                Subcategoria = !string.IsNullOrEmpty(request.Subcategoria) ? Guid.Parse(request.Subcategoria) : null,
                Preco = request.Preco ?? 0,
                PrecoOriginal = request.Preco,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Atualiza as propriedades do produto a partir de uma requisição de atualização.
        /// </summary>
        public void UpdateFromRequest(RequestUpdateProduto request)
        {
            if (request.Nome != null) Nome = request.Nome;
            if (request.Descricao != null) Descricao = request.Descricao;
            if (request.Categoria != null) Categoria = Guid.Parse(request.Categoria);
            if (request.Subcategoria != null) Subcategoria = !string.IsNullOrEmpty(request.Subcategoria) ? Guid.Parse(request.Subcategoria) : null;
            if (request.Preco.HasValue) Preco = request.Preco.Value;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
