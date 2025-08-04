using DaccApi.Helpers;
using DaccApi.Infrastructure.MercadoPago.Models;
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

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
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
                var orderId = await _ordersService.CreateOrderWithPayment(userId, request);
                return ResponseHelper.CreateSuccessResponse(new { OrderId = orderId });
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

    }
}
