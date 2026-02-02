namespace DaccApi.Model.Requests
{
    public class RequestQueryProjeto : BaseQueryRequest
    {
        public string? Status { get; set; }
        public string? Directorate { get; set; }
        public int? MinProgress { get; set; }
        public int? MaxProgress { get; set; }
    }
}
