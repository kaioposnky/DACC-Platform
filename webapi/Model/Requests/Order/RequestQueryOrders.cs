namespace DaccApi.Model.Requests.Order
{
    public class RequestQueryOrders : BaseQueryRequest
    {
        /// <summary>
        /// Filtro por status do pedido (ex: approved, pending, rejected).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Data inicial para filtro.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Data final para filtro.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
