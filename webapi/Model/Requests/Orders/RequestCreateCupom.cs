using DaccApi.Enum.Posts;
using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Orders
{
    public class RequestCreateCupom
    {
        [Required(ErrorMessage = "O código do cupom é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de desconto é obrigatório.")]
        public TipoDesconto DiscountType { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Value { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O limite de uso deve ser pelo menos 1.")]
        public int? UsageLimit { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
