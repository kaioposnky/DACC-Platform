using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de anúncios
/// </summary>
public static class AnuncioTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestAnuncio válido com dados padrão
    /// </summary>
    public static RequestAnuncio CreateValidAnuncio(
        string? title = null,
        string? content = null,
        string? type = null,
        bool isActive = true)
    {
        return new RequestAnuncio
        {
            Title = title ?? "Novo Evento na FATEC",
            Content = content ?? "Participe do nosso próximo evento sobre tecnologia e inovação! Vagas limitadas.",
            Type = type ?? "evento",
            IsActive = isActive,
            PrimaryButtonText = "Inscreva-se Agora",
            PrimaryButtonLink = "https://fatec.sp.gov.br/eventos/inscricao",
            SecondaryButtonText = "Saber Mais",
            SecondaryButtonLink = "https://fatec.sp.gov.br/eventos/detalhes",
            ImageUrl = "https://fatec.sp.gov.br/imagens/evento-tech.jpg",
            ImageAlt = "Banner do evento de tecnologia"
        };
    }

    /// <summary>
    /// Cria um anúncio com dados mínimos obrigatórios
    /// </summary>
    public static RequestAnuncio CreateMinimalAnuncio()
    {
        return new RequestAnuncio
        {
            Title = "Anúncio Mínimo",
            Content = "Conteúdo básico do anúncio de teste",
            Type = "noticia",
            IsActive = true
        };
    }

    /// <summary>
    /// Cria um anúncio com dados inválidos para testes de validação
    /// </summary>
    public static RequestAnuncio CreateInvalidAnuncio(string invalidField)
    {
        return invalidField switch
        {
            "titulo_vazio" => new RequestAnuncio
            {
                Title = "",
                Content = "Conteúdo válido",
                Type = "noticia",
                IsActive = true
            },
            "conteudo_vazio" => new RequestAnuncio
            {
                Title = "Título válido",
                Content = "",
                Type = "noticia",
                IsActive = true
            },
            _ => CreateValidAnuncio()
        };
    }

    /// <summary>
    /// Cria um anúncio para update
    /// </summary>
    public static RequestAnuncio CreateUpdateAnuncio(
        string? title = null,
        bool isActive = true)
    {
        return new RequestAnuncio
        {
            Title = title ?? "Anúncio Atualizado",
            Content = "Conteúdo atualizado do anúncio",
            Type = "aviso",
            IsActive = isActive,
            PrimaryButtonText = "Novo Botão",
            PrimaryButtonLink = "https://exemplo.com/novo"
        };
    }

    /// <summary>
    /// Cria múltiplos anúncios para testes em lote
    /// </summary>
    public static List<RequestAnuncio> CreateMultipleAnuncios(int count)
    {
        var anuncios = new List<RequestAnuncio>();

        for (int i = 1; i <= count; i++)
        {
            anuncios.Add(CreateValidAnuncio(
                title: $"Anúncio {i}",
                content: $"Conteúdo do anúncio número {i}",
                type: i % 2 == 0 ? "evento" : "noticia"
            ));
        }

        return anuncios;
    }
}
