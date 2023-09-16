using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class StadiumManager
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public Stadium? Stadium { get; set; }

        [Required]
        public IdentityUser? User { get; set; }
    }
}
