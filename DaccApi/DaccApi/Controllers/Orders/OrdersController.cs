using DaccApi.Exceptions;
using DaccApi.Helpers;
using DaccApi.Infrastructure.MercadoPago.Constants;
using DaccApi.Infrastructure.MercadoPago.Models;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Model.Requests;
using DaccApi.Services.Orders;
using Helpers.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Orders
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IMercadoPagoService _mercadoPagoService;

        public OrdersController(IOrdersService ordersService, IMercadoPagoService mercadoPagoService)
        {
            _ordersService = ordersService;
            _mercadoPagoService = mercadoPagoService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateOrderWithPayment([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (request.OrderItems.Count == 0)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST
                        , "Nenhum item foi adicionado ao pedido.");
                }
                
                var userId = ClaimsHelper.GetUserId(User);
                var orderResponse = await _ordersService.CreateOrderWithPayment(userId, request);
                return ResponseHelper.CreateSuccessResponse(new { orderResponse });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

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

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] Guid userId)
        {
            try
            {
                var orders = await _ordersService.GetOrdersByUserId(userId);
                return ResponseHelper.CreateSuccessResponse(orders);
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

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id, [FromBody] string status)
        {
            try
            {
                await _ordersService.UpdateOrderStatus(id, status);
                return ResponseHelper.CreateSuccessResponse(new { OrderId = id, Status = status });
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

        // Precisa deixar anonimo para liberar a API do mercadopago mandar o webhook
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> ProcessWebhook([FromBody] MercadoPagoWebHookRequest request)
        {
            try
            {
                var webhookSignatue = Request.Headers["x-signature"].FirstOrDefault();

                if (string.IsNullOrEmpty(webhookSignatue))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_WEBHOOK,
                        "Falha na validação da assinatura do webhook!");
                }

                // Retorna o body do webhook
                Request.Body.Position = 0;
                var webhookBody = await new StreamReader(Request.Body).ReadToEndAsync();

                var isValidWebhook = await _mercadoPagoService.ValidateWebhookSignatureAsync(
                    webhookBody, webhookSignatue);

                if (!isValidWebhook)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS,
                        "Falha na validação da assinatura do webhook!");
                }

                if (request.Type != MercadoPagoConstants.WebhookTypes.Payment)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
                }

                var paymentId = long.Parse(request.Data.Id);
                await _ordersService.ProcessWebhookPayment(paymentId);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Pagamento realizado com sucesso!");

            }
            catch (ProductOutOfStockException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.PRODUCT_OUT_OF_STOCK, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, ex.Message);
            }
        }
    }
}
