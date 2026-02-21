using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Classe helper para criar dados de teste de professores (faculty).
/// </summary>
public static class ProfessorTestDataBuilder
{
    /// <summary>
    /// Cria um objeto RequestProfessor válido com dados padrão.
    /// </summary>
    public static RequestProfessor CreateValidProfessor(
        string? name = null,
        string? title = null,
        string? position = null,
        string? specialization = null)
    {
        return new RequestProfessor
        {
            Name = name ?? "Prof. Dr. João Silva",
            Title = title ?? "Doutor",
            Position = position ?? "Professor Adjunto",
            Specialization = specialization ?? "Ciência da Computação e Engenharia de Software",
            Social = new SocialLinksRequest
            {
                Email = "joao.silva@fatec.sp.gov.br",
                Linkedin = "https://linkedin.com/in/joaosilva",
                Github = "https://github.com/joaosilva"
            }
        };
    }

    /// <summary>
    /// Cria um professor com dados mínimos obrigatórios.
    /// </summary>
    public static RequestProfessor CreateMinimalProfessor()
    {
        return new RequestProfessor
        {
            Name = "Professor Teste",
            Title = "Mestre",
            Position = "Docente",
            Specialization = "Sistemas de Informação",
            Social = new SocialLinksRequest()
        };
    }

    /// <summary>
    /// Cria um professor com dados inválidos para testes de validação.
    /// </summary>
    public static RequestProfessor CreateInvalidProfessor(string invalidField)
    {
        return invalidField switch
        {
            "nome_vazio" => new RequestProfessor
            {
                Name = "",
                Title = "Doutor",
                Position = "Professor",
                Specialization = "Computação",
                Social = new SocialLinksRequest()
            },
            "email_invalido" => new RequestProfessor
            {
                Name = "Teste",
                Title = "Doutor",
                Position = "Professor",
                Specialization = "Computação",
                Social = new SocialLinksRequest
                {
                    Email = "email-invalido"
                }
            },
            _ => CreateValidProfessor()
        };
    }

    /// <summary>
    /// Cria um professor para update.
    /// </summary>
    public static RequestProfessor CreateUpdateProfessor(
        string? name = null,
        string? position = null)
    {
        return new RequestProfessor
        {
            Name = name ?? "Prof. Dr. João Silva Atualizado",
            Title = "Doutor",
            Position = position ?? "Professor Titular",
            Specialization = "Inteligência Artificial aplicaca à Saúde",
            Social = new SocialLinksRequest
            {
                Email = "joao.atualizado@fatec.sp.gov.br",
                Linkedin = "https://linkedin.com/in/joaosilva-updated"
            }
        };
    }

    /// <summary>
    /// Cria múltiplos professores para testes em lote.
    /// </summary>
    public static List<RequestProfessor> CreateMultipleProfessores(int count)
    {
        var professores = new List<RequestProfessor>();

        for (int i = 1; i <= count; i++)
        {
            professores.Add(CreateValidProfessor(
                name: $"Prof. Professor {i}",
                position: i % 2 == 0 ? "Professor Adjunto" : "Professor Assistente",
                specialization: $"Área de Pesquisa {i}"
            ));
        }

        return professores;
    }
}
