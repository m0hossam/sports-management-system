using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class StadiumManager
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [ForeignKey("Stadium.Id")]
        public Stadium Stadium { get; set; } = null!;

        [Required]
        public IdentityUser? User { get; set; }
    }
}
