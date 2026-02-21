using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Model.Objects.Order;

using DaccApi.Model;

namespace DaccApi.Tests.Mocks
{
    public class MockMercadoPagoService : IMercadoPagoService
    {
        public Task<PaymentResponse> CreatePreferenceAsync(Order order, List<ProdutoVariacaoInfo> items, DateTime? expirationDate)
        {
            return Task.FromResult(new PaymentResponse 
            { 
                PreferenceId = "mock_pref_" + Guid.NewGuid(),
                PaymentUrl = "http://mock.mercadopago.com/checkout",
                Status = "pending"
            });
        }

        public Task<PaymentStatusResponse> GetPaymentStatusAsync(long paymentId)
        {
            return Task.FromResult(new PaymentStatusResponse
            {
                PaymentId = paymentId,
                Status = "approved",
                PaymentMethod = "credit_card",
                ExternalReference = Guid.NewGuid() // Mockar conforme necessidade
            });
        }

        public Task<bool> ValidateWebhookSignatureAsync(string payload, string signature, string requestId, string? dataId)
        {
            return Task.FromResult(true);
        }
    }
}
