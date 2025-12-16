using DaccApi.Enum.UserEnum;
using DaccApi.Helpers;

namespace DaccApi.Model
{
    /// <summary>
    /// Define as constantes para os cargos de usuário.
    /// </summary>
    public class CargoUsuario
    {
        /// <summary>
        /// Cargo de administrador com acesso total.
        /// </summary>
        public const string Administrador = "administrador";
        /// <summary>
        /// Cargo para membros da diretoria.
        /// </summary>
        public const string Diretor = "diretor";
        /// <summary>
        /// Cargo para alunos regulares.
        /// </summary>
        public const string Aluno = "aluno";
        /// <summary>
        /// Cargo para usuários não autenticados ou visitantes.
        /// </summary>
        public const string Visitante = "visitante";
    }
}