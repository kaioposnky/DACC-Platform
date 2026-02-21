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
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define a descrição da notícia.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define o conteúdo da notícia.
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor da notícia.
        /// </summary>
        public Guid? AuthorId { get; set; }
        /// <summary>
        /// Obtém ou define a categoria da notícia.
        /// </summary>
        public string? CategoryName { get; set; }
        /// <summary>
        /// Obtém ou define a data da última atualização.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Obtém ou define a data de publicação.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Obtém ou define o tempo de leitura em minutos.
        /// </summary>
        public int? ReadTime { get; set; }

        /// <summary>
        /// Obtém ou define a URL da imagem.
        /// </summary>
        public string? Image { get; set; }

        public string? Gradient { get; set; }
        public string? Icon { get; set; }
        public string? ReadMoreLink { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImageAlt { get; set; }

        /// <summary>
        /// Obtém ou define as tags da notícia.
        /// </summary>
        public string[]? Tags { get; set; }
        
        
    }
}