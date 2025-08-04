using DaccApi.Model.Responses;

namespace DaccApi.Model.Objects.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public long? MercadoPagoPaymentId { get; set; }
        public Guid? PreferenceId { get; set; }
        public string? PaymentMethod { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        
        public OrderResponse ToOrderResponse()
        {
            return new OrderResponse
            {
                Id = Id,
                UserId = UserId,
                OrderDate = OrderDate,
                Status = Status,
                TotalAmount = TotalAmount,
                OrderItems = OrderItems
            };
        }
    }
}