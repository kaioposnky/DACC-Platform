namespace DaccApi.Model
{
    public class User
    {
        public int? Id { get; set; } 
        public int? TypeId {  get; set; }
        public int UsuSitua { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }


    }
}
