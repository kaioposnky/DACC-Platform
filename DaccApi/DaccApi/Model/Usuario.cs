namespace DaccApi.Model
{
    using System.ComponentModel.DataAnnotations.Schema;
    public class Usuario
    {
        public int Id { get; set; }
        public int TypeId {  get; set; }
        public int Name { get; set; }
        public int Email { get; set; }
        public int Password { get; set; }
        public DateTime RegistrationDate { get; set; }


    }
}
