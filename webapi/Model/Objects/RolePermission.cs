namespace DaccApi.Model
{
    /// <summary>
    /// Representa a associação entre um perfil (role) e uma permissão.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Obtém ou define o nome do perfil.
        /// </summary>
        public string Nome { get; set;}
        /// <summary>
        /// Obtém ou define a descrição da permissão.
        /// </summary>
        public string Descricao { get; set;}
    }
}