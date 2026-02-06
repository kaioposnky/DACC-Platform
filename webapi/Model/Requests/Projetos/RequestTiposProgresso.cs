using System.ComponentModel.DataAnnotations;
using DaccApi.Model;

namespace DaccApi.Model.Requests.Projetos
{
    public class RequestCreateTipoProgresso
    {
        [Required(ErrorMessage = "O nome do tipo de progresso é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }

    public class RequestUpdateTipoProgresso
    {
        [Required(ErrorMessage = "O nome do tipo de progresso é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}

namespace DaccApi.Model.Responses.Projeto
{
    public class ResponseTipoProgresso
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ResponseTipoProgresso(TipoProgresso entity)
        {
            Id = entity.Id;
            Name = entity.Nome;
        }
    }
}
