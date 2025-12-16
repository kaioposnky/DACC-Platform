namespace DaccApi.Model.Responses;

/// <summary>
/// Representa a resposta de um diretor.
/// </summary>
public class ResponseDiretor
{
/// <summary>
/// Obtém ou define o ID do diretor.
/// </summary>
public Guid Id { get; set; }

/// <summary>
/// Obtém ou define o nome do diretor.
/// </summary>
public string? Name { get; set; }

/// <summary>
/// Obtém ou define o título (cargo) do diretor (frontend specific).
/// </summary>
public string? Title { get; set; }

/// <summary>
/// Obtém ou define a descrição/posição do diretor.
/// </summary>
public string? Description { get; set; } // Mapped from Descricao or used for Position

/// <summary>
/// Obtém ou define a posição do diretor (frontend specific).
/// </summary>
public string? Position { get; set; }

/// <summary>
/// Obtém ou define a especialização do diretor (frontend specific).
/// </summary>
public string? Specialization { get; set; }

/// <summary>
/// Obtém ou define a URL da imagem do diretor.
/// </summary>
public string? Image { get; set; }

/// <summary>
/// Obtém ou define os links sociais do diretor.
/// </summary>
public SocialLinks Social { get; set; }

/// <summary>
/// Construtor para mapear de uma entidade Diretor.
/// </summary>
/// <param name="diretor">A entidade Diretor de origem.</param>
public ResponseDiretor(Diretor diretor)
{
    Id = diretor.Id;
    Name = diretor.Nome;
    Description = diretor.Descricao; // Maps to general description
    Image = diretor.ImagemUrl;
    Social = new SocialLinks
    {
        Linkedin = diretor.LinkedinLink,
        Github = diretor.GithubLink,
        Email = diretor.Email
    };

   // Propriedades específicas do frontend sem correspondência direta
    Title = string.Empty; // Preencher via lógica adicional no frontend ou serviço
    Position = diretor.Descricao; // Pode ser mapeado da descrição
    Specialization = string.Empty; // Preencher via lógica adicional no frontend ou serviço
}

/// <summary>
/// Construtor sem parâmetros para deserialização
/// </summary>
public ResponseDiretor()
{
    Social = new SocialLinks(); // Initialize social links in default constructor
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
 
