using DaccApi.Model.Objects;

namespace DaccApi.Model.Responses.Curso
{
    public class ResponseCurso
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public ResponseCurso(Objects.Curso curso)
        {
            Id = curso.Id;
            Nome = curso.Nome;
        }
    }
}
