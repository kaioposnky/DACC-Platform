using DaccApi.Enum.UserEnum;

namespace DaccApi.Model
{
    public class Usuario
    {
        public Guid? Id { get; set; } 
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string? SenhaHash { get; set; }
        public string? ImagemUrl { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public TipoUsuarioEnum TipoUsuario {  get; set; }
    }
}
