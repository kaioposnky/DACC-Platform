using DaccApi.Model;

namespace DaccApi.Model.Responses.Usuario
{
    public class ResponseRole
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public ResponseRole(TipoUsuario tipoUsuario)
        {
            Id = tipoUsuario.Id;
            Nome = tipoUsuario.Nome;
        }
    }
}
