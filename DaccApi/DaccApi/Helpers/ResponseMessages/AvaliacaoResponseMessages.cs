namespace DaccApi.Helpers;

public abstract class AvaliacaoResponseMessages : ResponseMessages
{
    public class BadRequestMessages
    {
        public static string NULL_BODY = "Request inválido. Os campos ProductId, Commentary, Rating, UserId não podem ser nulos.";
        public static string NULL_PRODUCT_ID = "Request inválido. O campo ProductId não pode ser nulo.";
        public static string NULL_USER_ID = "Request inválido. O campo UserId não pode ser nulo.";
        public static string INVALID_RATING = "Request inválido. A nota da avaliação deve ser um valor entre 0 e 5!";
        public static string NONE_FOUND = "Nenhuma avaliação foi encontrada!";
        public static string NOT_FOUND = "Avaliação não encontrada!";
    }
}