using DaccApi.Enum.Produtos;
using DaccApi.Helpers;
using DaccApi.Model.Responses;

namespace DaccApi.Model
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double? Preco { get; set; }
        public double? PrecoOriginal { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        
        public List<ProdutoVariacao> Variacoes { get; set; } = new();
        
        public static ResponseProdutoVariacao MapToResponseVariacao(ProdutoVariacao variation)
        {
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
