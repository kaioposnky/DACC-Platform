using DaccApi.Enum.Produtos;

namespace DaccApi.Model
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double? Preco { get; set; }
        public int Estoque { get; set; }
        public string Cor { get; set; } // TODO: Criar classe ou alguma forma de controle de cores recebidas
        public ProdutosEnum.ProdutosTamanho Tamanho { get; set; }
        public bool? Genero { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
