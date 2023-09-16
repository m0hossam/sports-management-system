using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class SystemAdmin
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public IdentityUser? User { get; set; }
    }
}
