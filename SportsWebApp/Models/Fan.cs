using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class Fan
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string NationalId { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public bool IsBlocked { get; set; } = false;

        [Required]
        public required IdentityUser User { get; set; }
    }
}
