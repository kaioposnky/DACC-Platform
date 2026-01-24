using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

public static class AuthTestDataBuilder
{
    public static RequestRegistro CreateValidRegister(string email, string ra)
    {
        return new RequestRegistro
        {
            Nome = "Test",
            Sobrenome = "User",
            Email = email,
            Ra = ra,
            Curso = "Civil Engineering",
            Telefone = "11912345678",
            Senha = "Password@123",
            InscritoNoticia = true
        };
    }

    public static RequestLogin CreateValidLogin(string email, string senha)
    {
        return new RequestLogin
        {
            Email = email,
            Senha = senha
        };
    }
}
