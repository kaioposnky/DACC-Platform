namespace DaccApi.Model
{
    /// <summary>
    /// Representa um token de recuperação de senha.
    /// </summary>
    public class UsuarioResetToken
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime DataExpiracao { get; set; }
        public bool Usado { get; set; }
        public DateTime? DataUsada { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
