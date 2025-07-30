using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DaccApi.Helpers;
using DaccApi.Model.Validation;

namespace DaccApi.Model
{
    public class RequestUpdateProdutoVariacao
    {
        [ColorValidation]
        public string? Cor { get; set; }

        [AllowedValues("PP", "P", "M", "G", "GG", "XG", "Pequeno", "Medio", "Grande", 
            ErrorMessage = "Tamanho deve ser: PP, P, M, G, GG, XG, Pequeno, Medio ou Grande")]
        public string? Tamanho { get; set; }

        [Range(0, 99999, ErrorMessage = "Estoque deve estar entre 0 e 99.999 unidades")]
        public int? Estoque { get; set; }

        [Range(0, 999, ErrorMessage = "Ordem da variação deve estar entre 0 e 999")]
        public int? OrdemVariacao { get; set; }

        [ImageValidation(maxFileSize: 5 * 1024 * 1024, maxFileCount: 10)]
        public IFormFile[]? Imagens { get; set; }

        [MaxLength(10, ErrorMessage = "Máximo 10 textos alternativos para imagens")]
        public string[]? ImagensAlt { get; set; }
    }
}