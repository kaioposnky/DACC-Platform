namespace DaccApi.Responses
{
    public class ErrorRequest
    {
        public string Error { get; set; }
        public Exception Exception { get; set; }

        public ErrorRequest(string error, Exception exception)
        {
            Error = error;
            Exception = exception;
        }
    }
}
