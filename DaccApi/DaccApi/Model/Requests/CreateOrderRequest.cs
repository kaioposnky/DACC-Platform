namespace DaccApi.Model.Requests
{
    public class CreateOrderRequest
    {
        public List<OrderItemRequest> OrderItems { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public Guid ProductVariationId { get; set; }
        public int Quantity { get; set; }
    }
}