namespace DaccApi.Responses
{
    /// <summary>
    /// Classe base para todas as respostas da API
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Código identificador da resposta
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Mensagem descritiva da resposta
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Dados retornados (apenas para respostas de sucesso)
        /// </summary>
        public object? Data { get; set; }
        
        /// <summary>
        /// Detalhes adicionais (principalmente para erros de validação)
        /// </summary>
        public object[]? Details { get; set; }

        public ApiResponse(bool success, string code, string message, object? data = null, object[]? details = null)
        {
            Success = success;
            Code = code;
            Message = message;
            Data = data;
            Details = details;
        }
    }
}