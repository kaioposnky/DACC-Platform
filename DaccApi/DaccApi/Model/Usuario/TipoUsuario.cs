namespace DaccApi.Model;

public class TipoUsuario
{
    private string Value { get; }

    private TipoUsuario(string value)
    {
        Value = value;
    }

    public static readonly TipoUsuario Visitante = new TipoUsuario("visitante");
    public static readonly TipoUsuario Aluno = new TipoUsuario("aluno");
    public static readonly TipoUsuario Administrador = new TipoUsuario("administrador");
}