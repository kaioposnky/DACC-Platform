namespace DaccApi.Helpers;

/// <summary>
/// Contém mensagens de resposta padrão para o domínio de Avaliação.
/// </summary>
public abstract class AvaliacaoResponseMessages : ResponseMessages
{
    /// <summary>
    /// Mensagens para requisições inválidas (Bad Request).
    /// </summary>
    public new class BadRequestMessages
    {
        /// <summary>
        /// Mensagem para corpo da requisição nulo ou inválido.
        /// </summary>
        public static string NULL_BODY = "Request inválido. Os campos ProductId, Commentary, Rating, UserId não podem ser nulos.";
        /// <summary>
        /// Mensagem para quando o ID do produto é nulo.
        /// </summary>
        public static string NULL_PRODUCT_ID = "Request inválido. O campo ProductId não pode ser nulo.";
        /// <summary>
        /// Mensagem para quando o ID do usuário é nulo.
        /// </summary>
        public static string NULL_USER_ID = "Request inválido. O campo UserId não pode ser nulo.";
        /// <summary>
        /// Mensagem para quando a nota da avaliação é inválida.
        /// </summary>
        public static string INVALID_RATING = "Request inválido. A nota da avaliação deve ser um valor entre 0 e 5!";
        /// <summary>
        /// Mensagem para quando nenhuma avaliação é encontrada.
        /// </summary>
        public static string NONE_FOUND = "Nenhuma avaliação foi encontrada!";
        /// <summary>
        /// Mensagem para quando uma avaliação específica não é encontrada.
        /// </summary>
        public static string NOT_FOUND = "Avaliação não encontrada!";
    }
}