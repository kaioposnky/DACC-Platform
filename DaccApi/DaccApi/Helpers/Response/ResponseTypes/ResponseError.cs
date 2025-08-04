namespace Helpers.Response{
    public class ResponseError{
        public int StatusCode { get; set; }
        public Error ErrorInfo { get; set; }

        private ResponseError(int statusCode, string code, string message, object[]? details = null){
            StatusCode = statusCode;
            ErrorInfo = new Error(code, message, details);
        }

        public ResponseError WithDetails(object[]? details)
        {
            return new ResponseError(StatusCode, ErrorInfo.Code, ErrorInfo.Message, details);
        }
        
        public ResponseError WithDetails(ValidationErrorDetail[]? details)
        {
            return new ResponseError(StatusCode, ErrorInfo.Code, ErrorInfo.Message, details);
        }
        
        public static ResponseError WithDetails(ResponseError error, params object[]? details)
        {
            return new ResponseError(error.StatusCode, error.ErrorInfo.Code, error.ErrorInfo.Message, details);
        }

        public static ResponseError AUTH_TOKEN_INVALID = new ResponseError(401, "AUTH_TOKEN_INVALID", "Token JWT inválido", null);
        public static ResponseError AUTH_TOKEN_EXPIRED = new ResponseError(401, "AUTH_TOKEN_EXPIRED", "Token JWT expirado", null);
        public static ResponseError AUTH_INSUFFICIENT_PERMISSIONS = new ResponseError(403, "AUTH_INSUFFICIENT_PERMISSIONS", "Permissões insuficientes", null);
        public static ResponseError VALIDATION_ERROR = new ResponseError(400, "VALIDATION_ERROR", "Erro de validação dos dados", null);
        public static ResponseError RESOURCE_NOT_FOUND = new ResponseError(404, "RESOURCE_NOT_FOUND", "Recurso não encontrado", null);
        public static ResponseError RESOURCE_ALREADY_EXISTS = new ResponseError(409, "RESOURCE_ALREADY_EXISTS", "Recurso já existe", null);
        public static ResponseError RATE_LIMIT_EXCEEDED = new ResponseError(429, "RATE_LIMIT_EXCEEDED", "Limite de requisições excedido", null);
        public static ResponseError INVALID_CREDENTIALS = new ResponseError(401, "INVALID_CREDENTIALS", "Credenciais inválidas", null);
        public static ResponseError ACCOUNT_INACTIVE = new ResponseError(400, "ACCOUNT_INACTIVE", "Conta desativada", null);
        public static ResponseError INSUFFICIENT_STOCK = new ResponseError(400, "INSUFFICIENT_STOCK", "Estoque insuficiente", null);
        public static ResponseError PRODUCT_OUT_OF_STOCK = new ResponseError(400, "PRODUCT_OUT_OF_STOCK", "Produto fora de estoque", null);
        public static ResponseError CART_ITEM_NOT_FOUND = new ResponseError(404, "CART_ITEM_NOT_FOUND", "Item não encontrado no carrinho", null);
        public static ResponseError EVENT_FULL = new ResponseError(400, "EVENT_FULL", "Evento lotado", null);
        public static ResponseError REGISTRATION_CLOSED = new ResponseError(400, "REGISTRATION_CLOSED", "Inscrições encerradas", null);
        public static ResponseError INTERNAL_SERVER_ERROR = new ResponseError(500, "INTERNAL_SERVER_ERROR", "Erro interno do servidor", null);
        public static ResponseError BAD_REQUEST = new ResponseError(400, "BAD_REQUEST", "Dados inválidos na requisição", null);
        public static ResponseError CONTENT_TOO_LARGE = new ResponseError(413, "CONTENT_TOO_LARGE", "O arquivo enviado não pode ter mais de 5MB de tamanho.", null);
        public static readonly ResponseError PAYMENT_FAILED = new(400, "PAYMENT_FAILED", "Falha no processamento do pagamento");
        public static readonly ResponseError INVALID_WEBHOOK = new(400, "INVALID_WEBHOOK", "Webhook inválido");
        
        public class Error{
            public string Code { get; set; }
            public string Message { get; set; }
            public object[]? Details { get; set; }

            public Error(string code, string message, object[]? details){
                Code = code;
                Message = message;
                Details = details;
            }

        }
        
        public class ValidationErrorDetail
        {
            public string Field { get; set; }
            public string Message { get; set; }
        }
    }
}