using DaccApi.Model.Objects;

namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de um professor (faculty).
/// </summary>
public class ResponseProfessor
{
    /// <summary>
    /// Obtém ou define o ID do professor.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome do professor.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Obtém ou define o título acadêmico.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Obtém ou define o cargo.
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Obtém ou define a especialização.
    /// </summary>
    public string Specialization { get; set; }

    /// <summary>
    /// Obtém ou define a URL da imagem.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Obtém ou define os links sociais do professor.
    /// </summary>
    public SocialLinks Social { get; set; }

    /// <summary>
    /// Obtêm ou define a data de criação do professor
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Obtêm ou define a data de última atualização do professor
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Construtor para mapear de uma entidade Professor.
    /// </summary>
    /// <param name="professor">A entidade Professor de origem.</param>
    public ResponseProfessor(Professor professor)
    {
        Id = professor.Id;
        Name = professor.Nome;
        Title = professor.Titulo;
        Position = professor.Cargo;
        Specialization = professor.Especializacao;
        Image = professor.ImagemUrl;
        Social = new SocialLinks
        {
            Linkedin = professor.Linkedin,
            Github = professor.Github,
            Email = professor.Email
        };
        CreatedAt = professor.DataCriacao;
        UpdatedAt = professor.DataAtualizacao;
    }

    /// <summary>
    /// Construtor sem parâmetros para deserialização
    /// </summary>
    public ResponseProfessor()
    {
        Social = new SocialLinks();
    }
}

/// <summary>
/// Representa os links de redes sociais para o frontend.
/// </summary>
public class SocialLinks
{
    /// <summary>
    /// Obtém ou define o link para o perfil do LinkedIn.
    /// </summary>
    public string? Linkedin { get; set; }

    /// <summary>
    /// Obtém ou define o link para o perfil do GitHub.
    /// </summary>
    public string? Github { get; set; }

    /// <summary>
    /// Obtém ou define o endereço de e-mail.
    /// </summary>
    public string? Email { get; set; }
}
