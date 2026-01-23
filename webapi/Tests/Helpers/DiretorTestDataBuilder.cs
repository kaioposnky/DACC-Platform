using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de diretores
/// </summary>
public static class DiretorTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestDiretor válido com dados padrão
    /// </summary>
    public static RequestDiretor CreateValidDiretor(
        string? nome = null,
        string? titulo = null,
        string? cargo = null,
        string? especializacao = null)
    {
        return new RequestDiretor
        {
            Nome = nome ?? "Dr. João Silva",
            Titulo = titulo ?? "Doutor",
            Cargo = cargo ?? "Diretor Acadêmico",
            Especializacao = especializacao ?? "Ciência da Computação e Inteligência Artificial",
            Email = "joao.silva@fatec.sp.gov.br",
            Linkedin = "https://linkedin.com/in/joaosilva",
            Github = "https://github.com/joaosilva",
            ImageFile = null // Testes sem upload de arquivo por enquanto
        };
    }

    /// <summary>
    /// Cria um diretor com dados mínimos obrigatórios
    /// </summary>
    public static RequestDiretor CreateMinimalDiretor()
    {
        return new RequestDiretor
        {
            Nome = "Diretor Teste",
            Titulo = "Mestre",
            Cargo = "Coordenador",
            Especializacao = "Engenharia de Software",
            ImageFile = null
        };
    }

    /// <summary>
    /// Cria um diretor com dados inválidos para testes de validação
    /// </summary>
    public static RequestDiretor CreateInvalidDiretor(string invalidField)
    {
        return invalidField switch
        {
            "nome_vazio" => new RequestDiretor
            {
                Nome = "",
                Titulo = "Doutor",
                Cargo = "Diretor",
                Especializacao = "Computação",
                ImageFile = null
            },
            "email_invalido" => new RequestDiretor
            {
                Nome = "Teste",
                Titulo = "Doutor",
                Cargo = "Diretor",
                Especializacao = "Computação",
                Email = "email-invalido",
                ImageFile = null
            },
            _ => CreateValidDiretor()
        };
    }

    /// <summary>
    /// Cria um diretor para update
    /// </summary>
    public static RequestDiretor CreateUpdateDiretor(
        string? nome = null,
        string? cargo = null)
    {
        return new RequestDiretor
        {
            Nome = nome ?? "Dr. João Silva Atualizado",
            Titulo = "Doutor",
            Cargo = cargo ?? "Diretor Geral",
            Especializacao = "Inteligência Artificial e Machine Learning",
            Email = "joao.atualizado@fatec.sp.gov.br",
            Linkedin = "https://linkedin.com/in/joaosilva-updated",
            ImageFile = null
        };
    }

    /// <summary>
    /// Cria múltiplos diretores para testes em lote
    /// </summary>
    public static List<RequestDiretor> CreateMultipleDiretores(int count)
    {
        var diretores = new List<RequestDiretor>();

        for (int i = 1; i <= count; i++)
        {
            diretores.Add(CreateValidDiretor(
                nome: $"Dr. Diretor {i}",
                cargo: i % 2 == 0 ? "Diretor Acadêmico" : "Coordenador de Curso",
                especializacao: $"Área de Especialização {i}"
            ));
        }

        return diretores;
    }
}
