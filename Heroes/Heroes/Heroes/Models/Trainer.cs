using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Heroes.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Password must contain at least one capital letter, one digit, and one non-alphanumeric character.")]
        public string Password { get; set; }  // This should be hashed and stored securely

        public ICollection<Hero>? Heroes { get; set; } = new List<Hero>();
    }
}
