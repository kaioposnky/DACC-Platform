using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model
{
    public class RequestLogin
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}