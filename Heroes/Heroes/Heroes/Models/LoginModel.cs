using System.ComponentModel.DataAnnotations;

namespace Heroes.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }

}
