namespace DaccApi.Helpers;

public class ResponseMessages
{
    /// <summary>
    /// Mensagens para BadRequest
    /// </summary>
    public class BadRequestMessages
    {
        /// <summary>
        /// Mensagem de BadRequest genérica
        /// </summary>
        public const string GENERIC = "Request inválido.";
    }
    
    /// <summary>
    /// Mensagens para ErrorRequest
    /// </summary>
    public class ErrorRequestMessages
    {
        /// <summary>
        /// Mensagem de Error genérica
        /// </summary>
        public const string GENERIC = "Erro ao realizar operação!";
    }
    
    /// <summary>
    /// Mensagens para SuccessRequest
    /// </summary>
    public class SuccessRequestMessages
    {
        /// <summary>
        /// Mensagem de sucesso genérica
        /// </summary>
        public const string GENERIC = "Operação concluída com sucesso!";
    }
}