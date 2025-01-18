namespace DaccApi.Responses
{
    public class BadRequest
    {
        public string Error { get; set; }

        public BadRequest(string error)
        {
            Error = error;
        }
    }
}
