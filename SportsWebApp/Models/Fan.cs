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
        [Display(Name = "National ID")]
        public required string NationalId { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public bool IsBlocked { get; set; } = false;

        [Required]
        public required IdentityUser User { get; set; }
    }
}
