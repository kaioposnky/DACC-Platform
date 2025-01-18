namespace DaccApi.Responses.UserResponse
{
    public class UserResponseRequest
    {
        public string Message { get; set; }
        public object Data { get; set; }

        public UserResponseRequest(string message, object data)
        {
            Message = message;
            Data = data;
        }
    }
}
