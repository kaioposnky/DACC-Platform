using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

public static class AuthTestDataBuilder
{
    public static RequestRegistro CreateValidRegister(string email, string ra)
    {
        return new RequestRegistro
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Ra = ra,
            Course = "Civil Engineering",
            Phone = "11912345678",
            Password = "Password@123",
            IsSubscribedToNews = true
        };
    }

    public static RequestLogin CreateValidLogin(string email, string password)
    {
        return new RequestLogin
        {
            Email = email,
            Password = password
        };
    }
}
