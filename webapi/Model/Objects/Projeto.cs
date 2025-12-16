namespace DaccApi.Model
{
    /// <summary>
    /// Representa um projeto no sistema.
    /// </summary>
    public class Projeto
    {
        
        /// <summary>
        /// Obtém ou define o ID do projeto.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Obtém ou define o título do projeto.
        /// </summary>
        public string? Titulo { get; set; }
        
        /// <summary>
        /// Obtém ou define a descrição do projeto.
        /// </summary>
        public string? Descricao { get; set; }
        
        /// <summary>
        /// Obtém ou define a URL da imagem do projeto.
        /// </summary>
        public string? ImagemUrl { get; set; }
        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        public string? ImagemAlt { get; set; }
        
        /// <summary>
        /// Obtém ou define o status do projeto.
        /// </summary>
        public string? Status { get; set; }
        
        /// <summary>
        /// Obtém ou define a diretoria responsável pelo projeto.
        /// </summary>
        public string? Diretoria { get; set; }
        
        /// <summary>
        /// Obtém ou define as tags associadas ao projeto.
        /// </summary>
        public string[]? Tags { get; set; }

    }
}
