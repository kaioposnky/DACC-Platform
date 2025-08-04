using DaccApi.Model.Requests;
using NHibernate.Mapping;

namespace DaccApi.Model.Objects.Order
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public static List<OrderItem> FromRequestList(IEnumerable<OrderItemRequest> requestList)
        {
            // Seleciona todos os elementos passa eles para FromRequest e retorna o resultado adicionando eles na lista
            return requestList.Select(FromRequest).ToList();
        }
        
        public static OrderItem FromRequest(OrderItemRequest request)
        {
            return new OrderItem()
            {
                Id = Guid.Empty,
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                UnitPrice = request.UnitPrice
            };
        }
    }
}