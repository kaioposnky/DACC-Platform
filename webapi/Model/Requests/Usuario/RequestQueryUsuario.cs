namespace DaccApi.Model.Requests.Usuario
{
    public class RequestQueryUsuario : BaseQueryRequest
    {
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? Role { get; set; }
        public string? Course { get; set; }
        public bool? IsActive { get; set; }
    }
}
