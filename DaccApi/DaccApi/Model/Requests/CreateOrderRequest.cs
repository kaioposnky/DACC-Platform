namespace DaccApi.Model.Requests
{
    public class CreateOrderRequest
    {
        public decimal TotalAmount { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}