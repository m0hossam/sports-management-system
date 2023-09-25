using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class ClubRepresentative
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required Club Club { get; set; }

        [Required]
        public required IdentityUser User { get; set; }
    }
}
