namespace DaccApi.Model.Requests
{
    public class CreateOrderRequest
    {
        public List<OrderItemRequest> OrderItems { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid ProdutoId { get; set; }
        public Guid ProdutoVariacaoId { get; set; }
        public int Quantidade { get; set; }
    }
}