using DaccApi.Enum.UserEnum;

namespace DaccApi.Model
{
    public class RequestUsuario
    {
        public int? Id { get; set; } 
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Telefone { get; set; }
        public string? ImagemUrl { get; set; }
        public TipoUsuarioEnum? TipoUsuario {  get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
