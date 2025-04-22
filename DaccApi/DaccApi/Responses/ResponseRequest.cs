namespace DaccApi.Responses.UserResponse
{
    public class ResponseRequest
    {
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseRequest(string message, object data)
        {
            Message = message;
            Data = data;
        }
    }
}
