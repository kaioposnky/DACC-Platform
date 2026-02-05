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
        string? name = null,
        string? title = null,
        string? position = null,
        string? specialization = null)
    {
        return new RequestDiretor
        {
            Name = name ?? "Dr. João Silva",
            Title = title ?? "Doutor",
            Position = position ?? "Diretor Acadêmico",
            Specialization = specialization ?? "Ciência da Computação e Inteligência Artificial",
            Email = "joao.silva@fatec.sp.gov.br",
            Linkedin = "https://linkedin.com/in/joaosilva",
            Github = "https://github.com/joaosilva"
        };
    }

    /// <summary>
    /// Cria um diretor com dados mínimos obrigatórios
    /// </summary>
    public static RequestDiretor CreateMinimalDiretor()
    {
        return new RequestDiretor
        {
            Name = "Diretor Teste",
            Title = "Mestre",
            Position = "Coordenador",
            Specialization = "Engenharia de Software"
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
                Name = "",
                Title = "Doutor",
                Position = "Diretor",
                Specialization = "Computação"
            },
            "email_invalido" => new RequestDiretor
            {
                Name = "Teste",
                Title = "Doutor",
                Position = "Diretor",
                Specialization = "Computação",
                Email = "email-invalido"
            },
            _ => CreateValidDiretor()
        };
    }

    /// <summary>
    /// Cria um diretor para update
    /// </summary>
    public static RequestDiretor CreateUpdateDiretor(
        string? name = null,
        string? position = null)
    {
        return new RequestDiretor
        {
            Name = name ?? "Dr. João Silva Atualizado",
            Title = "Doutor",
            Position = position ?? "Diretor Geral",
            Specialization = "Inteligência Artificial e Machine Learning",
            Email = "joao.atualizado@fatec.sp.gov.br",
            Linkedin = "https://linkedin.com/in/joaosilva-updated"
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
                name: $"Dr. Diretor {i}",
                position: i % 2 == 0 ? "Diretor Acadêmico" : "Coordenador de Curso",
                specialization: $"Área de Especialização {i}"
            ));
        }

        return diretores;
    }
}
