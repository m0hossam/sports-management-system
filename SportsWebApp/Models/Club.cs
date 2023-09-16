using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }
    }
}
