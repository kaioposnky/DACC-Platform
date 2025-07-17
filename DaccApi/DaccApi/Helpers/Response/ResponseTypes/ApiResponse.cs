namespace Helpers.Response{
    public class ApiResponse{
        public bool Success { get; set; }
        public object? Response { get; set; }
        public ApiResponse(bool success, object? response){
            Success = success;
            Response = response;
        }
    }
}