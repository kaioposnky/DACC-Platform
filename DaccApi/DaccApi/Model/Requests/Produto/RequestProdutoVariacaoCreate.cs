using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DaccApi.Helpers;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestProdutoVariacaoCreate
    {
        [Required(ErrorMessage = "Nome da cor é obrigatório")]
        [ColorValidation]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Tamanho é obrigatório")]
        [AllowedValues("PP", "P", "M", "G", "GG", "XG", "Pequeno", "Medio", "Grande", 
            ErrorMessage = "Tamanho deve ser: PP, P, M, G, GG, XG, Pequeno, Medio ou Grande")]
        public string Tamanho { get; set; }

        [Range(0, 99999, ErrorMessage = "Estoque deve estar entre 0 e 99.999 unidades")]
        public int Estoque { get; set; } = 0;

        [Range(0, 999, ErrorMessage = "Ordem da variação deve estar entre 0 e 999")]
        public int OrdemVariacao { get; set; } = 0;

        [ImageValidation(maxFileSize: 5 * 1024 * 1024, maxFileCount: 10)]
        public IFormFile[] Imagens { get; set; } = Array.Empty<IFormFile>();

        [MaxLength(10, ErrorMessage = "Máximo 10 textos alternativos para imagens")]
        public string[]? ImagensAlt { get; set; } = Array.Empty<string>();
    }
}