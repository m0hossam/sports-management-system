using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class Fan
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? NationalId { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public IdentityUser? User { get; set; }
    }
}
