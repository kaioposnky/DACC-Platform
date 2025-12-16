using DaccApi.Enum.UserEnum;
using DaccApi.Model.Responses;
using NHibernate.Mapping;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa um usuário no sistema.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Obtém ou define o ID do usuário.
        /// </summary>
        public Guid Id { get; set; } 
        /// <summary>
        /// Obtém ou define o nome do usuário.
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Obtém ou define o sobrenome do usuário.
        /// </summary>
        public string Sobrenome { get; set; }
        /// <summary>
        /// Obtém ou define o e-mail do usuário.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Obtém ou define o RA (Registro do Aluno) do usuário.
        /// </summary>
        public string Ra { get; set; }
        /// <summary>
        /// Obtém ou define o curso do usuário.
        /// </summary>
        public string Curso { get; set; }
        /// <summary>
        /// Obtém ou define o telefone do usuário.
        /// </summary>
        public string Telefone { get; set; }
        /// <summary>
        /// Obtém ou define o hash da senha do usuário.
        /// </summary>
        public string? SenhaHash { get; set; }
        /// <summary>
        /// Obtém ou define a URL da imagem de perfil do usuário.
        /// </summary>
        public string? ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define se o usuário está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;
        /// <summary>
        /// Obtém ou define se o usuário está inscrito na newsletter.
        /// </summary>
        public bool? InscritoNoticia { get; set; } = false;
        /// <summary>
        /// Obtém ou define o cargo do usuário.
        /// </summary>
        public string Cargo {  get; set; }
        /// <summary>
        /// Obtém ou define a data de criação do usuário.
        /// </summary>
        public DateTime DataCriacao { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização do usuário.
        /// </summary>
        public DateTime DataAtualizacao { get; set; }

        /// <summary>
        /// Cria um objeto Usuário a partir de uma requisição de criação.
        /// </summary>
        public Usuario FromRequest(RequestCreateUsuario requestCreate)
        {
            return new Usuario()
            {
                Nome = requestCreate.Nome,
                Sobrenome = requestCreate.Sobrenome,
                Ra = requestCreate.Ra,
                Email = requestCreate.Email,
                Telefone = requestCreate.Telefone,
                InscritoNoticia = requestCreate.InscritoNoticia,
                SenhaHash = requestCreate.Senha,
                Curso = requestCreate.Curso
            };
        }

        /// <summary>
        /// Cria um objeto Usuário a partir de uma requisição de atualização.
        /// </summary>
        public Usuario FromUpdateRequest(RequestUpdateUsuario request)
        {
            return new Usuario()
            {
                Nome = request.Nome,
                Sobrenome = request.Sobrenome,
                Email = request.Email,
                Curso = request.Curso,
                Telefone = request.Telefone,
                InscritoNoticia = request.InscritoNoticia
            };
        }

        /// <summary>
        /// Converte uma lista de usuários para uma lista de objetos de resposta.
        /// </summary>
        public static List<ResponseUsuario> ToListResponse(List<Usuario> userList)
        {
            return userList.Select(user => user.ToResponse()).ToList();
        }
        
        /// <summary>
        /// Converte o objeto Usuário em um objeto de resposta.
        /// </summary>
        public ResponseUsuario ToResponse()
        {
            return new ResponseUsuario()
            {
                Nome = Nome,
                Sobrenome = Sobrenome,
                Ra = Ra,
                Email = Email,
                Telefone = Telefone,
                ImagemUrl = ImagemUrl,
                Cargo = Cargo,
                DataCriacao = DataCriacao,
                DataAtualizacao = DataAtualizacao
            };
        }
        
    }
}
