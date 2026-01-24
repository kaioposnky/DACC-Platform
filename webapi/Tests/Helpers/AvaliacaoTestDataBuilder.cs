using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de avaliações.
/// </summary>
public static class AvaliacaoTestDataBuilder
{
    public static RequestCreateAvaliacao CreateValidAvaliacao(Guid produtoId, double nota = 5, string? comentario = null)
    {
        return new RequestCreateAvaliacao
        {
            ProdutoId = produtoId,
            Nota = nota,
            Comentario = comentario ?? "Excelente produto! Recomendo muito."
        };
    }

    public static RequestUpdateAvaliacao CreateUpdateAvaliacao(double nota = 4, string? comentario = null)
    {
        return new RequestUpdateAvaliacao
        {
            Nota = nota,
            Comentario = comentario ?? "Atualizando minha opinião, ainda é bom."
        };
    }
}
