namespace Helpers.Response{
    public class ResponseSuccess{
        public int StatusCode { get; set; }
        public Success SuccessInfo { get; set; }

        private ResponseSuccess(int statusCode, string message, object? data = null){
            StatusCode = statusCode;
            SuccessInfo = new Success(message, data);
        }

        public static ResponseSuccess WithData(ResponseSuccess success, object data){
            return new ResponseSuccess(success.StatusCode, success.SuccessInfo.Message, data);
        }
        
        public ResponseSuccess WithData(object data){
            return new ResponseSuccess(this.StatusCode, this.SuccessInfo.Message, data);
        }
        
        public static ResponseSuccess OK = new ResponseSuccess(200, "Requisição bem-sucedida", null);
        public static ResponseSuccess CREATED = new ResponseSuccess(201, "Recurso criado com sucesso", null);
        public static ResponseSuccess NO_CONTENT = new ResponseSuccess(204, "Requisição bem-sucedida, mas não há conteúdo para retornar", null);

        public class Success{
            public string Message { get; set; }
            public object? Data { get; set; }

            public Success(string message, object? data){
                Message = message;
                Data = data;
            }
        }
    }
}