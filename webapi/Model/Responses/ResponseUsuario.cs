using DaccApi.Enum.UserEnum;

namespace DaccApi.Model.Responses
{
    public class ResponseUsuario
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Ra { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? ImagemUrl { get; set; }
        public string? Cargo {  get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}