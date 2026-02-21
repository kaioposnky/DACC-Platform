using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de avaliações.
/// </summary>
public static class AvaliacaoTestDataBuilder
{
    public static RequestCreateAvaliacao CreateValidAvaliacao(Guid productId, double rating = 5, string? comment = null, string? title = null)
    {
        return new RequestCreateAvaliacao
        {
            ProductId = productId,
            Rating = rating,
            Title = title ?? "Produto Excelente",
            Comment = comment ?? "Excelente produto! Recomendo muito."
        };
    }

    public static RequestUpdateAvaliacao CreateUpdateAvaliacao(double rating = 4, string? comment = null, string? title = null)
    {
        return new RequestUpdateAvaliacao
        {
            Rating = rating,
            Title = title ?? "Produto Bom",
            Comment = comment ?? "Atualizando minha opinião, ainda é bom."
        };
    }
}
