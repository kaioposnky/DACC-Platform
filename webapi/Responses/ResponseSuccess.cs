namespace DaccApi.Responses
{
    /// <summary>
    /// Classe para respostas de sucesso da API
    /// </summary>
    public class ResponseSuccess : ApiResponse
    {
        /// <summary>
        /// Código de status HTTP
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ResponseSuccess"/>.
        /// </summary>
        public ResponseSuccess(int statusCode, string code, string message, object? data = null)
            : base(true, code, message, data, null)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Cria uma nova instância de sucesso com dados
        /// </summary>
        public static ResponseSuccess WithData(ResponseSuccess success, object data)
        {
            return new ResponseSuccess(success.StatusCode, success.Code, success.Message, UnwrapSinglePropertyObject(data));
        }
        
        /// <summary>
        /// Cria uma nova instância de sucesso com dados
        /// </summary>
        public ResponseSuccess WithData(object data)
        {
            return new ResponseSuccess(StatusCode, Code, Message, UnwrapSinglePropertyObject(data));
        }

        /// <summary>
        /// Remove objetos wrapper desnecessários quando há apenas uma propriedade
        /// </summary>
        private static object UnwrapSinglePropertyObject(object data)
        {
            var type = data.GetType();
            
            // Se é um tipo anônimo com apenas uma propriedade, desencapsula automaticamente
            if (!type.Name.Contains("AnonymousType")) return data;
            var properties = type.GetProperties();

            if (properties.Length != 1) return data;
            var property = properties[0];
            return property.GetValue(data)!;
        }
        
        // Respostas de Sucesso Padrão
        /// <summary>Sucesso: A requisição foi bem-sucedida.</summary>
        public static readonly ResponseSuccess OK = new(200, "OK", "Requisição bem-sucedida");
        /// <summary>Sucesso: O recurso foi criado com sucesso.</summary>
        public static readonly ResponseSuccess CREATED = new(201, "CREATED", "Recurso criado com sucesso");
        /// <summary>Sucesso: A requisição foi bem-sucedida, mas não há conteúdo para retornar.</summary>
        public static readonly ResponseSuccess NO_CONTENT = new(204, "NO_CONTENT", "Requisição bem-sucedida, mas não há conteúdo para retornar");
    }
}