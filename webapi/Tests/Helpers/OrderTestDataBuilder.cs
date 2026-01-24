using DaccApi.Enum.Orders;
using DaccApi.Model.Requests;

namespace DaccApi.Tests.Helpers;

/// <summary>
/// Helper para construção de dados de teste para pedidos.
/// </summary>
public static class OrderTestDataBuilder
{
    public static CreateOrderRequest CreateValidOrder(
        Guid productVariationId, 
        Guid productId, 
        int quantity = 1,
        string? couponCode = null)
    {
        return new CreateOrderRequest
        {
            DeliveryMethod = DeliveryMethod.CampusDelivery,
            CouponCode = couponCode,
            Items = new List<CartItemRequest>
            {
                new()
                {
                    Id = productVariationId,
                    ProductId = productId,
                    Quantity = quantity
                }
            }
        };
    }

    public static CreateOrderRequest CreateEmptyOrder()
    {
        return new CreateOrderRequest
        {
            DeliveryMethod = DeliveryMethod.CampusDelivery,
            Items = new List<CartItemRequest>()
        };
    }
}
