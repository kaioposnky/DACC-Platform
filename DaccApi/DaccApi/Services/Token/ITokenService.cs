using DaccApi.Model;

namespace DaccApi.Services.Token
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario, HashSet<string> permissions);
    }
}