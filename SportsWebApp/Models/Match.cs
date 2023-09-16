using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class Match
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public Club? HomeClub { get; set; }

        [Required]
        public Club? AwayClub { get; set; }

        public Stadium? Stadium { get; set; }
    }
}
