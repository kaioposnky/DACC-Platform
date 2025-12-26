using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests;

public class BaseQueryRequest
{
    /// <summary>
    /// Obtém ou define o termo de pesquisa.
    /// </summary>
    [StringLength(200, ErrorMessage = "Termo de busca deve ter no máximo 200 caracteres")]
    public string? SearchQuery { get; set; }

    /// <summary>
    /// Obtém ou define o número da página.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Obtém ou define o limite de itens por página.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Limite deve estar entre 1 e 100")]
    public int Limit { get; set; } = 16;
}