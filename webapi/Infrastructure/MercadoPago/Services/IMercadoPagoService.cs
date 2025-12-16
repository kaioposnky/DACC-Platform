using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Services.MercadoPago
{
    /// <summary>
    /// Define a interface para o serviço de interação com a API do Mercado Pago.
    /// </summary>
    public interface IMercadoPagoService
    {
        /// <summary>
        /// Cria uma preferência de pagamento no Mercado Pago.
        /// </summary>
        Task<PaymentResponse> CreatePreferenceAsync(Order order, List<ProdutoVariacaoInfo> variations,
            DateTime? expireDate);
        /// <summary>
        /// Obtém o status de um pagamento a partir de seu ID.
        /// </summary>
        Task<PaymentStatusResponse> GetPaymentStatusAsync(long paymentId);
        /// <summary>
        /// Valida a assinatura de um webhook do Mercado Pago.
        /// </summary>
        Task<bool> ValidateWebhookSignatureAsync(string body, string signature, string requestId, string dataId);
    }
}