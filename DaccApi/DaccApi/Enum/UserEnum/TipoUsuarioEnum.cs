using System.ComponentModel;

namespace DaccApi.Enum.UserEnum
{
    public enum TipoUsuarioEnum
    {
        [Description("visitante")]
        Visitante,
        
        [Description("aluno")]
        Aluno,
        
        [Description("diretor")]
        Diretor,
        
        [Description("administrador")]
        Administrador
    }
}