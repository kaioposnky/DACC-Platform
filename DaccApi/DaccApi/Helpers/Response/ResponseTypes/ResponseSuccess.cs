namespace Helpers.Response{
    public class ResponseSuccess{
        public int StatusCode { get; set; }
        public Success Success { get; set; }

        private ResponseSuccess(int statusCode, string message, object? data = null){
            StatusCode = statusCode;
            Success = new Success(message, data);
        }

        public static ResponseSuccess WithData(ResponseSuccess success, object data){
            return new ResponseSuccess(success.StatusCode, success.Success.Message, data);
        }

        public static ResponseSuccess OK = new ResponseSuccess(200, "Requisição bem-sucedida", null);
        public static ResponseSuccess CREATED = new ResponseSuccess(201, "Recurso criado com sucesso", null);
        public static ResponseSuccess NO_CONTENT = new ResponseSuccess(204, "Requisição bem-sucedida, mas não há conteúdo para retornar", null);

        private class Success{
            public string Message { get; set; }
            public object? Data { get; set; }

            private Success(string message, object? data){
                Message = message;
                Data = data;
            }
        }
    }
}