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
        public string? Title { get; set; }
        /// <summary>
        /// Obtém ou define a descrição do evento.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Obtém ou define a data do evento.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do evento.
        /// </summary>
        public string? EventType { get; set; }
        /// <summary>
        /// Obtém ou define o texto do botão de ação.
        /// </summary>
        public string? ActionText { get; set; }
        /// <summary>
        /// Obtém ou define o link da ação.
        /// </summary>
        public string? ActionLink { get; set; }

    }
}