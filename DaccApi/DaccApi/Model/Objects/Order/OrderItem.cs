using DaccApi.Model.Requests;
using NHibernate.Mapping;

namespace DaccApi.Model.Objects.Order
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ProdutoVariacaoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

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
                ProdutoVariacaoId = request.ProdutoVariacaoId,
                Quantidade = request.Quantidade,
                ProdutoId = request.ProdutoId,
                PrecoUnitario = 0
            };
        }
    }
}