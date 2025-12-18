namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar ou atualizar um evento.
    /// </summary>
    public class RequestEvento
    {
        /// <summary>
        /// Obtém ou define o ID do evento.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Obtém ou define o título do evento.
        /// </summary>
        public string? Titulo { get; set; }
        /// <summary>
        /// Obtém ou define a descrição do evento.
        /// </summary>
        public string? Descricao { get; set; }
        /// <summary>
        /// Obtém ou define a data do evento.
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do evento.
        /// </summary>
        public string? TipoEvento { get; set; }
        /// <summary>
        /// Obtém ou define o texto do botão de ação.
        /// </summary>
        public string? TextoAcao { get; set; }
        /// <summary>
        /// Obtém ou define o link da ação.
        /// </summary>
        public string? LinkAcao { get; set; }

    }
}