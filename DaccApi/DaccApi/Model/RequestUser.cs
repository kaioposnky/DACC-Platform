namespace DaccApi.Model
{
    public class RequestUsuario
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid? UserId { get; set; }
        public int TypeId { get; set; }
    }
}
