namespace DaccApi.Model.Requests
{
    public class RequestQueryAvaliacao : BaseQueryRequest
    {
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
