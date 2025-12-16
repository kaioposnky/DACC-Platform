namespace DaccApi.Model
{
    /// <summary>
    /// Representa um evento no sistema.
    /// </summary>
    public class Evento

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
        public DateTime? Data { get; set; }
        /// <summary>
        /// Obtém ou define o tipo do evento.
        /// </summary>
        public string? TipoEvento { get; set; }
        /// <summary>
        /// Obtém ou define o ID do autor do evento.
        /// </summary>
        public Guid? AutorId { get; set; }
        /// <summary>
        /// Obtém ou define o texto para o botão de ação.
        /// </summary>
        public string? TextoAcao { get; set; }
        /// <summary>
        /// Obtém ou define o link para a ação.
        /// </summary>
        public string? LinkAcao { get; set; }
    
    }
}