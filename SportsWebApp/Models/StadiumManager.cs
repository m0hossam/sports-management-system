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
        public required string Name { get; set; }

        [Required]
        public required int StadiumId { get; set; }
        public Stadium Stadium { get; set; } = null!;

        [Required]
        public required IdentityUser User { get; set; }
    }
}
