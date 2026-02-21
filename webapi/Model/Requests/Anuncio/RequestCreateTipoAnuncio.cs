using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Anuncio
{
    public class RequestCreateTipoAnuncio
    {
        [Required(ErrorMessage = "O nome do tipo de anúncio é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome do tipo de anúncio deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}
