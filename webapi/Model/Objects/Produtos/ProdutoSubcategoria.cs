using System.ComponentModel.DataAnnotations.Schema;
using DaccApi.Model.Responses;

namespace DaccApi.Model;

/// <summary>
/// Representa a entidade de Subcategoria de um produto
/// </summary>
[Table("produto_subcategoria")]
public class ProdutoSubcategoria
{
    /// <summary>
    /// Id da subcategoria
    /// </summary>
    [Column("id")]
    public Guid? Id { get; set; }
    /// <summary>
    /// Nome da subcategoria
    /// </summary>
    [Column("nome")]
    public string Nome { get; set; }
    /// <summary>
    /// Id da categoria da subcategoria
    /// </summary>
    [Column("categoria_id")]
    public Guid? CategoriaId { get; set; }


    public ResponseProdutoSubcategoria ToResponse()
    {
        return new ResponseProdutoSubcategoria
        {
            Id = Id,
            Name =  Nome,
            CategoryId = CategoriaId
        };
    }
}