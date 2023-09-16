using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class Stadium
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
