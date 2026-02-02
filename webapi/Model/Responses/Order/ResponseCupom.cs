using System.Text.Json.Serialization;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Model.Responses.Order
{
    /// <summary>
    /// Representa a resposta de um cupom de desconto.
    /// </summary>
    public class ResponseCupom
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("discountType")]
        public string DiscountType { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("usageLimit")]
        public int? UsageLimit { get; set; }

        [JsonPropertyName("currentUsage")]
        public int CurrentUsage { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        public ResponseCupom(Cupom cupom)
        {
            Id = cupom.Id;
            Code = cupom.Codigo;
            DiscountType = cupom.TipoDesconto.ToString();
            Value = cupom.Valor;
            ExpirationDate = cupom.DataExpiracao;
            UsageLimit = cupom.LimiteUso;
            CurrentUsage = cupom.UsoAtual;
            Active = cupom.Ativo;
        }

        public ResponseCupom() { }
    }
}
