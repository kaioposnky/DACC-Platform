namespace DaccApi.Model
{
    public class RequestResetPassword
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
