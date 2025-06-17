namespace DaccApi.Model
{
    public class RequestUsuario
    {
        public Guid Id { get; set; } 
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string ImagemUrl { get; set; }
        public TipoUsuario TipoUsuario {  get; set; }
    }
}
