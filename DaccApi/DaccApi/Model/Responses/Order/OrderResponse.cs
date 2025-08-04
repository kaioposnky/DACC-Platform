using DaccApi.Model.Objects.Order;

namespace DaccApi.Model.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
