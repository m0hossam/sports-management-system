using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class ClubRepresentative
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public Club? Club { get; set; }

        [Required]
        public IdentityUser? User { get; set; }
    }
}
