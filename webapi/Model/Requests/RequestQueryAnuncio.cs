namespace DaccApi.Model.Requests
{
    public class RequestQueryAnuncio : BaseQueryRequest
    {
        public string? Type { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public bool? IsActive { get; set; }
    }
}
