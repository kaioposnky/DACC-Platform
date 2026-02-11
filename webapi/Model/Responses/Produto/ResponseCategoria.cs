using DaccApi.Model;

namespace DaccApi.Model.Responses.Produto
{
    /// <summary>
    /// Representa uma categoria de produto para o frontend.
    /// </summary>
    public class ResponseCategoria
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ResponseCategoria(ProdutoCategoria categoria)
        {
            Id = categoria.Id;
            Name = categoria.Nome;
        }

        public ResponseCategoria()
        {
            Name = string.Empty;
        }
    }
}
