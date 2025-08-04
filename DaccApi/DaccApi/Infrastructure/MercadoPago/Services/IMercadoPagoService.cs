using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Services.MercadoPago
{
    public interface IMercadoPagoService
    {
        Task<PaymentResponse> CreatePreferenceAsync(Order order);
        Task<PaymentStatusResponse> GetPaymentStatusAsync(long paymentId);
        Task<bool> ValidateWebhookSignatureAsync(string body, string signature);
    }
}