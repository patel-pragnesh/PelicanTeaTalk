using System;
using System.ComponentModel.DataAnnotations;

namespace WPSTORE.Models.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
