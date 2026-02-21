using DaccApi.Enum.Posts;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Model.Responses.Order
{
    public class ResponseCupom
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? UsageLimit { get; set; }
        public int CurrentUsage { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }
        public DateTime CreatedAt { get; set; }

        public ResponseCupom(Cupom cupom)
        {
            Id = cupom.Id;
            Code = cupom.Codigo;
            DiscountType = cupom.TipoDesconto.ToString();
            Value = cupom.Valor;
            ExpirationDate = cupom.DataExpiracao;
            UsageLimit = cupom.LimiteUso;
            CurrentUsage = cupom.UsoAtual;
            IsActive = cupom.Ativo;
            CreatedAt = cupom.DataCriacao;

            // Calcula se é válido hoje
            IsValid = IsActive && 
                      (!ExpirationDate.HasValue || ExpirationDate.Value > DateTime.UtcNow) && 
                      (!UsageLimit.HasValue || CurrentUsage < UsageLimit.Value);
        }
    }
}
