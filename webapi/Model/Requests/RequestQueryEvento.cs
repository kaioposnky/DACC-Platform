namespace DaccApi.Model.Requests
{
    public class RequestQueryEvento : BaseQueryRequest
    {
        public string? Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? Data { get; set; }
    }
}
