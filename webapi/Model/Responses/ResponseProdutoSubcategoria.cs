namespace DaccApi.Model.Responses;

public class ResponseProdutoSubcategoria
{
    /// <summary>
    /// Id da subcategoria
    /// </summary>
    public Guid? Id { get; set; }
    /// <summary>
    /// Nome da subcategoria
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Id da categoria da subcategoria
    /// </summary>
    public Guid? CategoryId { get; set; }
}