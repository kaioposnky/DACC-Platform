namespace DaccApi.Helpers;

public class AvaliacaoResponseMessages
{
    public class BadRequest
    {
        public static string GENERAL = "Request inválido.";
        public static string NULL_BODY = "Request inválido. Os campos ProductId, Commentary, Rating, UserId não podem ser nulos.";
        public static string NULL_PRODUCT_ID = "Request inválido. O campo ProductId não pode ser nulo.";
        public static string NULL_USER_ID = "Request inválido. O campo UserId não pode ser nulo.";
        public static string INVALID_RATING = "Request inválido. A nota da avaliação deve ser um valor entre 0 e 5!";
        public static string NONE_FOUND = "Nenhuma avaliação foi encontrada!";
        public static string NOT_FOUND = "Avaliação não encontrada!";
    }

    public class SuccessRequest
    {
        public static string GENERAL = "Operação concluída com sucesso!";
    }
    
    public class ErrorRequest
    {
        public static string GENERAL = "Erro ao realizar operação!";
    }
}