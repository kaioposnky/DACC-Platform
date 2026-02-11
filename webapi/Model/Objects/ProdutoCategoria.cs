namespace DaccApi.Model
{
    /// <summary>
    /// Representa uma categoria de produto.
    /// </summary>
    public class ProdutoCategoria
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
