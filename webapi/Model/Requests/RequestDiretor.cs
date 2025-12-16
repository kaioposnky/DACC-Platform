using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um diretor.
    /// </summary>
    public class RequestDiretor
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
            /// Obtém ou define a descrição do diretor.
            /// </summary>
            public string? Descricao { get; set; }
            /// <summary>
            /// Obtém ou define o arquivo de imagem do diretor.
            /// </summary>
            [ImageValidation]
            public IFormFile? ImageFile { get; set; }
            /// <summary>
            /// Obtém ou define o ID do usuário associado.
            /// </summary>
            public Guid? UsuarioId { get; set; }
            /// <summary>
            /// Obtém ou define o ID da diretoria.
            /// </summary>
            public Guid? DiretoriaId { get; set; }
            /// <summary>
            /// Obtém ou define o e-mail do diretor.
            /// </summary>
            public string? Email { get; set; }
            /// <summary>
            /// Obtém ou define o link do perfil do GitHub.
            /// </summary>
            public string? GithubLink    { get; set; }
            /// <summary>
            /// Obtém ou define o link do perfil do LinkedIn.
            /// </summary>
            public string? LinkedinLink   { get; set; }
    }
    
}
