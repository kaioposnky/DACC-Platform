using DaccApi.Enum.UserEnum;

namespace DaccApi.Model.Responses
{
    public class ResponseUsuario
    {
        public int? Id { get; set; } 
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? ImagemUrl { get; set; }
        public string? TipoUsuario {  get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}