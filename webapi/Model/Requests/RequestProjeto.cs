using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um projeto.
    /// </summary>
    public class RequestProjeto
    {
        /// <summary>
        /// Obtém ou define o título do projeto.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Obtém ou define a descrição do projeto.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Obtém ou define o status do projeto.
        /// </summary>
        public string? Status { get; set; }
        
        /// <summary>
        /// Obtém ou define a diretoria responsável.
        /// </summary>
        public string? Department { get; set; }
        /// <summary>
        /// Obtém ou define as tags do projeto.
        /// </summary>
        public string[]? Tags { get; set; }

        /// <summary>
        /// Obtém ou define o texto de conclusão do projeto.
        /// </summary>
        public string? CompletionText { get; set; }

        /// <summary>
        /// Obtém ou define o progresso do projeto (0-100).
        /// </summary>
        public int? Progress { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem do projeto.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
