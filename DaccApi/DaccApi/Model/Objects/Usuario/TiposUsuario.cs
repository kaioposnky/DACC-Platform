using DaccApi.Enum.UserEnum;
using DaccApi.Helpers;

namespace DaccApi.Model
{
    public class TiposUsuario
    {
        public const string Administrador = "administrador";
        public const string Diretor = "diretor";
        public const string Aluno = "aluno";
        public const string Visitante = "visitante";
        
        public static string FromEnum(TipoUsuarioEnum tipo)
        {
            return tipo.GetDescription();
        }

        public static TipoUsuarioEnum ToEnum(string tipo)
        {
            return tipo switch
            {
                Administrador => TipoUsuarioEnum.Administrador,
                Diretor => TipoUsuarioEnum.Diretor,
                Aluno => TipoUsuarioEnum.Aluno,
                Visitante => TipoUsuarioEnum.Visitante,
                _ => throw new ArgumentException("Tipo de usuário inválido")
            };
        }
    }
}