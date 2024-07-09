

using Heroes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Heroes.Models
{
    public class SignupModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Password must be at least 8 characters long and contain one capital letter, one digit, and one non-alphanumeric character.")]
        public string Password { get; set; }
    }
}
