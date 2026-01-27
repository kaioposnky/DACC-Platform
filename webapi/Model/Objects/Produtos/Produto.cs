using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
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
        public decimal Preco { get; set; }

        /// <summary>
        /// Obtém ou define o preço original do produto (para promoções).
        /// </summary>
        [Column("preco_original")]
        public decimal? PrecoOriginal { get; set; }

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

        // Propriedades adicionais para o frontend
        
        [Column("descricao_detalhada")]
        public string? DescricaoDetalhada { get; set; }
        
        [NotMapped]
        public List<string>? PerfeitoPara { get; set; }
        
        [Column("destaque")]
        public bool Destaque { get; set; }
        
        [NotMapped]
        public decimal AvaliacaoMedia { get; set; }
        
        [NotMapped]
        public int NumeroAvaliacoes { get; set; }
        
        [NotMapped]
        public List<AvaliacaoProduto>? Avaliacoes { get; set; }
        
        [NotMapped]
        public List<ProdutoEspecificacao>? Especificacoes { get; set; }
        
        [NotMapped]
        public ProdutoInformacaoEnvio? InformacaoEnvio { get; set; }

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
        public static Produto FromRequest(RequestCreateProduto request, Guid productId, Guid categoryId, Guid? subcategoryId)
        {
            return new Produto
            {
                Id = productId,
                Nome = request.Name,
                Descricao = request.Description,
                Categoria = categoryId,
                Subcategoria = subcategoryId,
                Preco = Convert.ToDecimal(request.Price),
                PrecoOriginal = Convert.ToDecimal(request.Price),
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                DescricaoDetalhada = request.DetailedDescription,
                PerfeitoPara = request.PerfectFor,
                Destaque = request.Featured,
                Especificacoes = request.Specifications?.Select(e => new ProdutoEspecificacao { Id = Guid.NewGuid(), ProdutoId = productId, Nome = e.Name, Valor = e.Value }).ToList(),
                InformacaoEnvio = request.ShippingInfo != null ? new ProdutoInformacaoEnvio
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = productId,
                    FreteGratis = request.ShippingInfo.FreeShipping,
                    DiasEstimados = request.ShippingInfo.EstimatedDays,
                    CustoEnvio = request.ShippingInfo.ShippingCost,
                    PoliticaDevolucao = request.ShippingInfo.ReturnPolicy,
                    Garantia = request.ShippingInfo.Warranty
                } : null
            };
        }

        /// <summary>
        /// Atualiza as propriedades do produto a partir de uma requisição de atualização.
        /// </summary>
        public void UpdateFromRequest(RequestUpdateProduto request, Guid? categoryId = null, Guid? subcategoryId = null)
        {
            if (request.Name != null) Nome = request.Name;
            if (request.Description != null) Descricao = request.Description;
            if (categoryId.HasValue) Categoria = categoryId.Value;
            if (request.Subcategory != null) Subcategoria = subcategoryId;
            if (request.Price.HasValue) Preco = Convert.ToDecimal(request.Price.Value);
            if (request.OriginalPrice.HasValue) PrecoOriginal = Convert.ToDecimal(request.OriginalPrice.Value);
            if (request.DetailedDescription != null) DescricaoDetalhada = request.DetailedDescription;
            if (request.PerfectFor != null) PerfeitoPara = request.PerfectFor;
            if (request.Featured.HasValue) Destaque = request.Featured.Value;
            if (request.Specifications != null) Especificacoes = request.Specifications.Select(e => new ProdutoEspecificacao { Id = Guid.NewGuid(), ProdutoId = Id, Nome = e.Name, Valor = e.Value }).ToList();
            if (request.ShippingInfo != null) InformacaoEnvio = new ProdutoInformacaoEnvio
            {
                Id = InformacaoEnvio?.Id ?? Guid.NewGuid(),
                ProdutoId = Id,
                FreteGratis = request.ShippingInfo.FreeShipping,
                DiasEstimados = request.ShippingInfo.EstimatedDays,
                CustoEnvio = request.ShippingInfo.ShippingCost,
                PoliticaDevolucao = request.ShippingInfo.ReturnPolicy,
                Garantia = request.ShippingInfo.Warranty
            };
            
            DataAtualizacao = DateTime.UtcNow;
        }
    }

    [Table("produto_especificacao")]
    public class ProdutoEspecificacao
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("produto_id")]
        public Guid ProdutoId { get; set; }
        
        [Column("nome")]
        public string Nome { get; set; }
        
        [Column("valor")]
        public string Valor { get; set; }
    }

    [Table("produto_informacao_envio")]
    public class ProdutoInformacaoEnvio
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("produto_id")]
        public Guid ProdutoId { get; set; }
        
        [Column("frete_gratis")]
        public bool FreteGratis { get; set; }
        
        [Column("dias_estimados")]
        public int DiasEstimados { get; set; }
        
        [Column("custo_envio")]
        public decimal? CustoEnvio { get; set; }
        
        [Column("politica_devolucao")]
        public string PoliticaDevolucao { get; set; }
        
        [Column("garantia")]
        public string? Garantia { get; set; }
    }
}
