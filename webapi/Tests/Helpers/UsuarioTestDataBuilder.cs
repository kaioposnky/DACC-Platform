using DaccApi.Model;

namespace DaccApi.Tests.Helpers;

public static class UsuarioTestDataBuilder
{
    public static RequestUpdateUsuario CreateUpdateUsuario(string? nome = null)
    {
        return new RequestUpdateUsuario
        {
            Name = nome ?? "Updated Name",
            LastName = "Updated Lastname",
            Phone = "11988887777",
            Course = "Engenharia de Software"
        };
    }
}
