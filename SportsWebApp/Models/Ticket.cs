using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public required int FanId { get; set; }
        public required Fan Fan { get; set; }

        [Required]
        public required int MatchId { get; set; }
        public Match Match { get; set; } = null!;
    }
}
