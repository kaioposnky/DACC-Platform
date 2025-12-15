namespace DaccApi.Responses
{
    /// <summary>
    /// Classe para respostas de erro da API
    /// </summary>
    public class ResponseError : ApiResponse
    {
        /// <summary>
        /// Código de status HTTP
        /// </summary>
        public int StatusCode { get; }

        public ResponseError(int statusCode, string code, string message, object[]? details = null)
            : base(false, code, message, null, details)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Cria uma nova instância do erro com detalhes adicionais
        /// </summary>
        public ResponseError WithDetails(object[]? details)
        {
            return new ResponseError(StatusCode, Code, Message, details);
        }
        
        /// <summary>
        /// Cria uma nova instância do erro com detalhes de validação
        /// </summary>
        public ResponseError WithDetails(ValidationErrorDetail[]? details)
        {
            return new ResponseError(StatusCode, Code, Message, details);
        }
        
        /// <summary>
        /// Método estático para criar erro com detalhes
        /// </summary>
        public static ResponseError WithDetails(ResponseError error, params object[]? details)
        {
            return new ResponseError(error.StatusCode, error.Code, error.Message, details);
        }

        // Erros de Autenticação (4xx)
        public static readonly ResponseError AUTH_TOKEN_INVALID = new(401, "AUTH_TOKEN_INVALID", "Token JWT inválido");
        public static readonly ResponseError AUTH_TOKEN_EXPIRED = new(401, "AUTH_TOKEN_EXPIRED", "Token JWT expirado");
        public static readonly ResponseError AUTH_INSUFFICIENT_PERMISSIONS = new(403, "AUTH_INSUFFICIENT_PERMISSIONS", "Permissões insuficientes");
        public static readonly ResponseError INVALID_CREDENTIALS = new(401, "INVALID_CREDENTIALS", "Credenciais inválidas");

        // Erros de Validação (4xx)
        public static readonly ResponseError VALIDATION_ERROR = new(400, "VALIDATION_ERROR", "Erro de validação dos dados");
        public static readonly ResponseError BAD_REQUEST = new(400, "BAD_REQUEST", "Dados inválidos na requisição");
        public static readonly ResponseError RESOURCE_NOT_FOUND = new(404, "RESOURCE_NOT_FOUND", "Recurso não encontrado");
        public static readonly ResponseError RESOURCE_ALREADY_EXISTS = new(409, "RESOURCE_ALREADY_EXISTS", "Recurso já existe");

        // Erros Específicos do Domínio (4xx)
        public static readonly ResponseError ACCOUNT_INACTIVE = new(400, "ACCOUNT_INACTIVE", "Conta desativada");
        public static readonly ResponseError INSUFFICIENT_STOCK = new(400, "INSUFFICIENT_STOCK", "Estoque insuficiente");
        public static readonly ResponseError PRODUCT_OUT_OF_STOCK = new(400, "PRODUCT_OUT_OF_STOCK", "Produto fora de estoque");
        public static readonly ResponseError CART_ITEM_NOT_FOUND = new(404, "CART_ITEM_NOT_FOUND", "Item não encontrado no carrinho");
        public static readonly ResponseError EVENT_FULL = new(400, "EVENT_FULL", "Evento lotado");
        public static readonly ResponseError REGISTRATION_CLOSED = new(400, "REGISTRATION_CLOSED", "Inscrições encerradas");
        public static readonly ResponseError CONTENT_TOO_LARGE = new(413, "CONTENT_TOO_LARGE", "O arquivo enviado não pode ter mais de 5MB de tamanho");
        public static readonly ResponseError PAYMENT_FAILED = new(400, "PAYMENT_FAILED", "Falha no processamento do pagamento");
        public static readonly ResponseError INVALID_WEBHOOK = new(400, "INVALID_WEBHOOK", "Webhook inválido");

        // Erros Técnicos (5xx)
        public static readonly ResponseError INTERNAL_SERVER_ERROR = new(500, "INTERNAL_SERVER_ERROR", "Erro interno do servidor");
        public static readonly ResponseError RATE_LIMIT_EXCEEDED = new(429, "RATE_LIMIT_EXCEEDED", "Limite de requisições excedido");
        
        /// <summary>
        /// Classe para detalhes de erro de validação
        /// </summary>
        public class ValidationErrorDetail
        {
            public required string Field { get; set; }
            public required string Message { get; set; }
        }
    }
}