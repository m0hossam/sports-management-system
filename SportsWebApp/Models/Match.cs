using Microsoft.EntityFrameworkCore;
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

        public int NumberOfAttendees { get; set; } = 0;

        public Club? HomeClub { get; set; }

        public Club? AwayClub { get; set; }

        public Stadium? Stadium { get; set; } = null; // null if stadium hasn't been decided yet
    }
}
