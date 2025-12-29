using DaccApi.Exceptions;
using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.MercadoPago.Constants;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model.Requests;
using DaccApi.Model.Requests.Order;
using DaccApi.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Responses;

namespace DaccApi.Controllers.Orders
{
    /// <summary>
    /// Controlador para gerenciar pedidos e pagamentos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly ICupomService _cupomService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="OrdersController"/>.
        /// </summary>
        public OrdersController(IOrdersService ordersService, IMercadoPagoService mercadoPagoService, ICupomService cupomService)
        {
            _ordersService = ordersService;
            _mercadoPagoService = mercadoPagoService;
            _cupomService = cupomService;
        }

        /// <summary>
        /// Cria um novo pedido e inicia o processo de pagamento.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        public async Task<IActionResult> CreateOrderWithPayment([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (request.Items.Count == 0)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST
                        , "Nenhum item foi adicionado ao pedido.");
                }

                var userId = ClaimsHelper.GetUserId(User);
                var orderResponse = await _ordersService.CreateOrderWithPayment(userId, request);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(orderResponse), "Pedido criado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (ProductOutOfStockException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.PRODUCT_OUT_OF_STOCK, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Busca pedidos com filtros.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("search")]
        public async Task<IActionResult> SearchOrders([FromQuery] RequestQueryOrders requestQuery)
        {
            try
            {
                var userId = ClaimsHelper.GetUserId(User);
                var orders = await _ordersService.SearchOrders(userId, requestQuery);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(orders));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Obtém um pedido específico pelo seu ID.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            try
            {
                var order = await _ordersService.GetOrderById(id);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { orders = order }));
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os pedidos de um usuário específico.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] Guid userId)
        {
            try
            {
                var orders = await _ordersService.GetOrdersByUserId(userId);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(orders));
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza o status de um pedido existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id, [FromBody] string status)
        {
            try
            {
                await _ordersService.UpdateOrderStatus(id, status);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData( new { OrderId = id, Status = status }));
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Valida um cupom de desconto.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("coupons/{code}")]
        public async Task<IActionResult> ValidateCupom([FromRoute] string code)
        {
            return await _cupomService.ValidateCupom(code);
        }

        /// <summary>
        /// Processa webhooks de pagamento do Mercado Pago.
        /// </summary>
        // Precisa deixar anonimo para liberar a API do mercadopago mandar o webhook
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> ProcessWebhook([FromForm] MercadoPagoWebHookRequest request)
        {
            try
            {
                // Ler o body raw primeiro
                Request.Body.Position = 0;
                var webhookBody = await new StreamReader(Request.Body).ReadToEndAsync();
                
                // Webhook de Merchant Order, pode ignorar
                if (!string.IsNullOrEmpty(request.Topic))
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
                }
                
                var webhookSignature = Request.Headers["x-signature"];

                if (string.IsNullOrEmpty(webhookSignature))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_WEBHOOK,
                        "Falha na validação da assinatura do webhook!");
                }

                var requestId = Request.Headers["x-request-id"].FirstOrDefault();
                var dataId = request.Data?.Id;

                var isValidWebhook = await _mercadoPagoService.ValidateWebhookSignatureAsync(
                    webhookBody, webhookSignature.FirstOrDefault(), requestId, dataId);

                if (!isValidWebhook)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS,
                        "Falha na validação da assinatura do webhook!");
                }

                if (request.Type != MercadoPagoConstants.WebhookTypes.Payment)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
                }

                if (request.Data?.Id == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Payment ID não encontrado");
                }
                
                var paymentId = long.Parse(request.Data.Id);
                await _ordersService.ProcessWebhookPayment(paymentId);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Pagamento realizado com sucesso!");

            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, ex.Message);
            }
        }
    }
}
