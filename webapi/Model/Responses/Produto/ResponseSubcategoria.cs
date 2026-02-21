using DaccApi.Model;

namespace DaccApi.Model.Responses.Produto
{
    /// <summary>
    /// Representa uma subcategoria de produto para o frontend.
    /// </summary>
    public class ResponseSubcategoria
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }

        public ResponseSubcategoria(ProdutoSubcategoria subcategoria)
        {
            Id = subcategoria.Id ?? Guid.Empty;
            Name = subcategoria.Nome;
            CategoryId = subcategoria.CategoriaId ?? Guid.Empty;
        }

        public ResponseSubcategoria()
        {
            Name = string.Empty;
        }
    }
}
