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
        string? titulo = null,
        string? conteudo = null,
        string? tipo = null,
        bool ativo = true)
    {
        return new RequestAnuncio
        {
            Titulo = titulo ?? "Novo Evento na FATEC",
            Conteudo = conteudo ?? "Participe do nosso próximo evento sobre tecnologia e inovação! Vagas limitadas.",
            TipoAnuncio = tipo ?? "evento",
            Ativo = ativo,
            BotaoPrimarioTexto = "Inscreva-se Agora",
            BotaoPrimarioLink = "https://fatec.sp.gov.br/eventos/inscricao",
            BotaoSecundarioTexto = "Saber Mais",
            BotaoSecundarioLink = "https://fatec.sp.gov.br/eventos/detalhes",
            ImagemUrl = "https://fatec.sp.gov.br/imagens/evento-tech.jpg",
            ImagemAlt = "Banner do evento de tecnologia"
        };
    }

    /// <summary>
    /// Cria um anúncio com dados mínimos obrigatórios
    /// </summary>
    public static RequestAnuncio CreateMinimalAnuncio()
    {
        return new RequestAnuncio
        {
            Titulo = "Anúncio Mínimo",
            Conteudo = "Conteúdo básico do anúncio de teste",
            TipoAnuncio = "noticia",
            Ativo = true
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
                Titulo = "",
                Conteudo = "Conteúdo válido",
                TipoAnuncio = "noticia",
                Ativo = true
            },
            "conteudo_vazio" => new RequestAnuncio
            {
                Titulo = "Título válido",
                Conteudo = "",
                TipoAnuncio = "noticia",
                Ativo = true
            },
            _ => CreateValidAnuncio()
        };
    }

    /// <summary>
    /// Cria um anúncio para update
    /// </summary>
    public static RequestAnuncio CreateUpdateAnuncio(
        string? titulo = null,
        bool ativo = true)
    {
        return new RequestAnuncio
        {
            Titulo = titulo ?? "Anúncio Atualizado",
            Conteudo = "Conteúdo atualizado do anúncio",
            TipoAnuncio = "aviso",
            Ativo = ativo,
            BotaoPrimarioTexto = "Novo Botão",
            BotaoPrimarioLink = "https://exemplo.com/novo"
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
                titulo: $"Anúncio {i}",
                conteudo: $"Conteúdo do anúncio número {i}",
                tipo: i % 2 == 0 ? "evento" : "noticia"
            ));
        }

        return anuncios;
    }
}
