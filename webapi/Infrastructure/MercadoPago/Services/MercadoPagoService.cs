using System.Security.Cryptography;
using System.Text;
using DaccApi.Helpers;
using DaccApi.Infrastructure.MercadoPago.Constants;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;

namespace DaccApi.Infrastructure.MercadoPago.Services
{
    namespace DaccApi.Infrastructure.MercadoPago.Services
    {
        /// <summary>
        /// Implementação do serviço para interagir com a API do Mercado Pago.
        /// </summary>
        public class MercadoPagoService : IMercadoPagoService
        {
            private readonly PreferenceClient _preferenceClient;
            private readonly PaymentClient _paymentClient;
            private readonly IUsuarioRepository _usuarioRepository;
            private readonly string _webhookSecret;
            private readonly string _applicationUrl;
            private readonly bool _sandboxMode;

            /// <summary>
            /// Inicia uma nova instância da classe <see cref="MercadoPagoService"/>.
            /// </summary>
            public MercadoPagoService(
                IConfiguration configuration,
                IUsuarioRepository usuarioRepository)
            {
                MercadoPagoConfig.AccessToken = configuration["MercadoPago:AccessToken"];
                _webhookSecret = configuration["MercadoPago:WebhookSecret"]!;
                _applicationUrl = configuration["ApplicationURL"]!;
                _sandboxMode = configuration["MercadoPago:UseSandbox"] == "true"; // Se vai usar dados reais ou de teste

                _usuarioRepository = usuarioRepository;

                _preferenceClient = new PreferenceClient();
                _paymentClient = new PaymentClient();
            }

            /// <summary>
            /// Cria uma preferência de pagamento no Mercado Pago.
            /// </summary>
            public async Task<PaymentResponse> CreatePreferenceAsync(Order order, List<ProdutoVariacaoInfo> variations,
                DateTime? expireDate = null)
            {
                try
                {
                    var user = await _usuarioRepository.GetUserById(order.UserId);

                    if (user == null)
                    {
                        throw new ArgumentException("Usuário não encontrado!");
                    }

                    // Dict com variações para ser O(1)
                    var variationDict = variations.ToDictionary(v => v.VariationId, v => v);

                    // Se não for especificado é 1 hora
                    expireDate ??= DateTime.Now + TimeSpan.FromHours(1);

                    // Uma preference determina:
                    // O que vai ser pago: produtos, quantidades, preços
                    // Quem vai pagar: email do comprador
                    // Como vai pagar: métodos permitidos
                    // Para onde redirecionar: URLs de sucesso, falha, pendente
                    // Outras configurações: parcelas, desconto, etc.
                    var preferenceRequest = new PreferenceRequest
                    {
                        // 4. Criar items sem loops aninhados
                        Items = order.OrderItems.Select(item =>
                        {
                            var variation = variationDict[item.ProdutoVariacaoId];

                            return new PreferenceItemRequest
                            {
                                Id = variation.VariationId.ToString(),
                                PictureUrl = variation.ImageUrl,
                                Title = $"{variation.ProductName} - {variation.ColorName} {variation.SizeName}",
                                Quantity = item.Quantidade,
                                CurrencyId = "BRL",
                                UnitPrice = item.PrecoUnitario,
                            };
                        }).ToList(),

                        Payer = new PreferencePayerRequest
                        {
                            Name = user.Nome,
                            Surname = user.Sobrenome,
                            Email = user.Email,
                            Phone = new PhoneRequest
                            {
                                AreaCode = user.Telefone[..2],
                                Number = user.Telefone[3..]
                            },

                        },

                        ExternalReference = order.Id.ToString(),

                        // URL onde o Webhook do pagamento será disparado
                        NotificationUrl = $"{_applicationUrl}/api/orders/webhook",

                        BackUrls = new PreferenceBackUrlsRequest
                        {
                            // URL de redirecionamento para cada resposta do pagamento
                            Success = $"{_applicationUrl}/api/payments/success",
                            Failure =
                                $"{_applicationUrl}/api/payments/failure", // Caso cliente clique voltar para o pagamento
                            Pending =
                                $"{_applicationUrl}/api/payments/pending" // Caso cliente crie o pagamento mas deixe para pagar depois
                        },

                        // Retorna automaticamente para o site
                        AutoReturn = "approved",

                        PaymentMethods = new PreferencePaymentMethodsRequest
                        {
                            // No futuro caso seja necessário excluir algum tipo de pagamento basta descomentar essas linhas
                            // ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                            // ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>(),

                            // Número máximo de parcelas no cartão de crédito
                            Installments = 6
                        },
                        // Data de expiração para realizar o pagamento (1h para realizar o pagamento)
                        ExpirationDateFrom = DateTime.UtcNow,
                        ExpirationDateTo = expireDate,
                        DateOfExpiration = expireDate,
                    };

                    // Cria a preferência pelo MercadoPago
                    var preference = await RetryHelper.ExecuteAndRetryAsync(async () =>
                        await _preferenceClient.CreateAsync(preferenceRequest));

                    return new PaymentResponse
                    {
                        PreferenceId = preference.Id,
                        // Envia URL de sandbox (de mentira) se habilitado ao invés do real
                        PaymentUrl = _sandboxMode ? preference.SandboxInitPoint : preference.InitPoint,
                        Status = MercadoPagoConstants.PaymentStatus.Pending
                    };
                }

                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erro ao criar preferência: {ex.Message} {ex.StackTrace}", ex);
                }
            }

            /// <summary>
            /// Obtém o status de um pagamento a partir de seu ID.
            /// </summary>
            public async Task<PaymentStatusResponse> GetPaymentStatusAsync(long paymentId)
            {
                try
                {
                    // Busca os dados do pagamento no MercadoPago
                    var payment = await RetryHelper.ExecuteAndRetryAsync(async () =>
                        await _paymentClient.GetAsync(paymentId));

                    return new PaymentStatusResponse()
                    {
                        PaymentId = payment.Id!.Value,
                        Status = payment.Status,
                        ExternalReference = Guid.Parse(payment.ExternalReference),
                        TransactionAmount = payment.TransactionAmount!.Value,
                        PaymentMethod = payment.PaymentMethodId,
                        DateCreated = payment.DateCreated!.Value,
                        DateApproved = payment.DateApproved,
                    };
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erro ao buscar pagamento: {ex.Message}", ex);
                }
            }

            /// <summary>
            /// Valida a assinatura de um webhook do Mercado Pago para garantir sua autenticidade.
            /// </summary>
            public async Task<bool> ValidateWebhookSignatureAsync(string body, string signature, string requestId,
                string dataId)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(requestId) ||
                        string.IsNullOrWhiteSpace(dataId))
                    {
                        return false;
                    }

                    // Extrair timestamp e hash da signature do MercadoPago
                    var parts = signature.Split(',');
                    if (parts.Length != 2)
                    {
                        return false;
                    }

                    var timestamp = parts[0].Replace("ts=", "");
                    var receivedHash = parts[1].Replace("v1=", "");

                    var manifest = $"id:{dataId};request-id:{requestId};ts:{timestamp};";

                    var computedHash = await ComputeHmacSha256Async(manifest, _webhookSecret);

                    return string.Equals(receivedHash, computedHash, StringComparison.OrdinalIgnoreCase);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private static async Task<string> ComputeHmacSha256Async(string data, string secret)
            {
                var keyBytes = Encoding.UTF8.GetBytes(secret);
                var dataBytes = Encoding.UTF8.GetBytes(data);

                using var hmac = new HMACSHA256(keyBytes);
                var hashBytes = await Task.Run(() => hmac.ComputeHash(dataBytes));
                return Convert.ToHexString(hashBytes).ToLowerInvariant();
            }
        }
    }
}