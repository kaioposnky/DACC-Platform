namespace DaccApi.Enum.UserEnum
{
    /// <summary>
    /// Contém enums relacionados ao usuário.
    /// </summary>
    public class UserEnum
    {
        /// <summary>
        /// Define os tipos de usuário.
        /// </summary>
        public enum UserEnumTypeId 
        {
            /// <summary>
            /// Usuário com permissões de administrador mestre.
            /// </summary>
            Master = 1,

        }
        /// <summary>
        /// Define a situação do usuário no sistema.
        /// </summary>
        public enum UserSituacao
        { 
            /// <summary>
            /// O usuário está ativo no sistema.
            /// </summary>
            Ativo = 1,
            /// <summary>
            /// O usuário está inativo no sistema.
            /// </summary>
            Inativo = 0
        }
    }
}
