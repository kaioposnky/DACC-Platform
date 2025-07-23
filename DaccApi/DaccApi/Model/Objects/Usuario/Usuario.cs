using DaccApi.Enum.UserEnum;
using DaccApi.Model.Responses;
using NHibernate.Mapping;

namespace DaccApi.Model
{
    public class Usuario
    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Ra { get; set; }
        public string Curso { get; set; }
        public string Telefone { get; set; }
        public string? SenhaHash { get; set; }
        public string ImagemUrl { get; set; }
        public bool? Ativo { get; set; }
        public bool? NewsLetterSubscriber { get; set; }
        public string Cargo {  get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public Usuario FromRequest(RequestUsuario request)
        {
            return new Usuario()
            {
                Nome = request.Nome,
                Sobrenome = request.Sobrenome,
                Ra = request.Ra,
                Email = request.Email,
                Telefone = request.Telefone,
                ImagemUrl = request.ImagemUrl,
                Cargo = request.Cargo,
                NewsLetterSubscriber = request.NewsLetterSubscriber,
                SenhaHash = request.Senha,
                Curso = request.Curso
            };
        }

        public Usuario FromUpdateRequest(RequestUpdateUsuario request)
        {
            return new Usuario()
            {
                Nome = request.Nome,
                Sobrenome = request.Sobrenome,
                Email = request.Email,
                Curso = request.Curso,
                Telefone = request.Telefone,
                ImagemUrl = request.ImagemUrl,
                NewsLetterSubscriber = request.NewsLetterSubscriber
            };
        }

        public static List<ResponseUsuario> ToListResponse(List<Usuario> userList)
        {
            return userList.Select(user => user.ToResponse()).ToList();
        }
        
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

