using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class SystemAdmin
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required IdentityUser User { get; set; }
    }
}
