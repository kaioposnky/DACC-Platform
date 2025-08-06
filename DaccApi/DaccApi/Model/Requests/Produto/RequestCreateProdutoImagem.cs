using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    public class RequestCreateProdutoImagem
    {
        [Required(ErrorMessage = "Arquivo de imagem é obrigatório")]
        public IFormFile ImageFile { get; set; }

        [StringLength(255, ErrorMessage = "Texto alternativo deve ter no máximo 255 caracteres")]
        public string? ImagemAlt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ordem deve ser um valor positivo")]
        public int Ordem { get; set; } = 0;
    }
}