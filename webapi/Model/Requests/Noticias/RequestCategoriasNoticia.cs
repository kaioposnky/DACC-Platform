using System.ComponentModel.DataAnnotations;
using DaccApi.Model;

namespace DaccApi.Model.Requests.Noticias
{
    public class RequestCreateCategoriaNoticia
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }

    public class RequestUpdateCategoriaNoticia
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}

namespace DaccApi.Model.Responses.Noticia
{
    public class ResponseCategoriaNoticia
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ResponseCategoriaNoticia(CategoriaNoticia entity)
        {
            Id = entity.Id;
            Name = entity.Nome;
        }
    }
}
