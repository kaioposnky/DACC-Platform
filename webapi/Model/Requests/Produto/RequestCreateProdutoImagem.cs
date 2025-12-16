using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    /// <summary>
    /// Representa a requisição para criar uma nova imagem de produto.
    /// </summary>
    public class RequestCreateProdutoImagem
    {
        /// <summary>
        /// Obtém ou define o arquivo de imagem.
        /// </summary>
        [Required(ErrorMessage = "Arquivo de imagem é obrigatório")]
        public IFormFile Imagem { get; set; }

        /// <summary>
        /// Obtém ou define o texto alternativo da imagem.
        /// </summary>
        [StringLength(255, ErrorMessage = "Texto alternativo deve ter no máximo 255 caracteres")]
        public string? ImagemAlt { get; set; }

        /// <summary>
        /// Obtém ou define a ordem de exibição da imagem.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Ordem deve ser um valor positivo")]
        public int Ordem { get; set; } = 0;
    }
}