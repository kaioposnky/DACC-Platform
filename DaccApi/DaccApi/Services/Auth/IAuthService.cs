using DaccApi.Model;

namespace DaccApi.Services.Auth
{
    public interface IAuthService
    {
        string GenerateToken(object request);
        bool ValidateCredentials(RequestUsuario request);
    }
}
