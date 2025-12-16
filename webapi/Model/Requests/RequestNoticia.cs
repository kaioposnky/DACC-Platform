using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar uma notícia.
    /// </summary>
    public class RequestNoticia
    {

        /// <summary>
        /// Obtém ou define o título da notícia.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define a descrição da notícia.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define o conteúdo da notícia.
        /// </summary>
        public string? Conteudo { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor da notícia.
        /// </summary>
        public Guid? AutorId { get; set; }
        /// <summary>
        /// Obtém ou define a categoria da notícia.
        /// </summary>
        public string? Categoria { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
        /// <summary>
        /// Obtém ou define a data de publicação.
        /// </summary>
        public DateTime? DataPublicacao { get; set; }
        
        
    }
}