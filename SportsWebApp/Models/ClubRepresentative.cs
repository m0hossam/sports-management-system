using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class ClubRepresentative
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [ForeignKey("Club.Id")]
        public Club Club { get; set; } = null!;

        [Required]
        public IdentityUser? User { get; set; }
    }
}
