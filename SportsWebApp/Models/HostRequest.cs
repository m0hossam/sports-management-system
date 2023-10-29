using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class HostRequest
    {
        public int Id { get; set; }

        public bool? IsApproved { get; set; } = null; //null if unhandled

        [Required]
        public required int ClubRepresentativeId { get; set; }
        public ClubRepresentative? ClubRepresentative { get; set; } = null!;

        [Required]
        public required int MatchId { get; set; }
        public Match? Match { get; set; } = null!;

        [Required]
        public required int StadiumId { get; set; }
        public Stadium? Stadium { get; set; } = null!;
    }
}
