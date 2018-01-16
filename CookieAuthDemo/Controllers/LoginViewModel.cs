using System.ComponentModel.DataAnnotations;

namespace CookieAuthDemo.Controllers
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}