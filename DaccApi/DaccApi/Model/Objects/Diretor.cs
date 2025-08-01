namespace DaccApi.Model
{
    public class Diretor
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemUrl { get; set; }
        public int? UsuarioId { get; set; }
        public Guid? DiretoriaId { get; set; }
        public string? Email { get; set; }
        public string? GithubLink    { get; set; }
        public string? LinkedinLink   { get; set; }
    }
}
