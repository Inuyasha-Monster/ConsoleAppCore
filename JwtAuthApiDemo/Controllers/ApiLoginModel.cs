using System.ComponentModel.DataAnnotations;

namespace JwtAuthApiDemo.Controllers
{
    public class ApiLoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}