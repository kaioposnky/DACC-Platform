namespace DaccApi.Model
{
    /// <summary>
    /// Representa um membro da diretoria do DACC.
    /// </summary>
    public class Diretor
    {
        /// <summary>
        /// Obtém ou define o ID do diretor.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o nome do diretor.
        /// </summary>
        public string? Nome { get; set; }
        /// <summary>
        /// Obtém ou define a descrição ou cargo do diretor.
        /// </summary>
        public string? Descricao { get; set; }
        /// <summary>
        /// Obtém ou define a URL da imagem do diretor.
        /// </summary>
        public string? ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define o ID do usuário associado.
        /// </summary>
        public Guid? UsuarioId { get; set; }
        /// <summary>
        /// Obtém ou define o ID da diretoria à qual pertence.
        /// </summary>
        public Guid? DiretoriaId { get; set; }
        /// <summary>
        /// Obtém ou define o e-mail de contato do diretor.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Obtém ou define o link para o perfil do GitHub.
        /// </summary>
        public string? GithubLink    { get; set; }
        /// <summary>
        /// Obtém ou define o link para o perfil do LinkedIn.
        /// </summary>
        public string? LinkedinLink   { get; set; }
    }
}
