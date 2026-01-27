using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de avaliações.
/// </summary>
public static class AvaliacaoTestDataBuilder
{
    public static RequestCreateAvaliacao CreateValidAvaliacao(Guid productId, double rating = 5, string? comment = null)
    {
        return new RequestCreateAvaliacao
        {
            ProductId = productId,
            Rating = rating,
            Comment = comment ?? "Excelente produto! Recomendo muito."
        };
    }

    public static RequestUpdateAvaliacao CreateUpdateAvaliacao(double rating = 4, string? comment = null)
    {
        return new RequestUpdateAvaliacao
        {
            Rating = rating,
            Comment = comment ?? "Atualizando minha opinião, ainda é bom."
        };
    }
}
