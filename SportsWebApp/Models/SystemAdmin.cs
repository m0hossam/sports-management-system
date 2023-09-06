using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class SystemAdmin
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Remote(action: "VerifyUsername", controller: "Register")] // Verifies username is not taken
        public string? Username { get; set; }

        [Required]
        [MaxLength(20), MinLength(8, ErrorMessage = "Password must be atleast 8 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}