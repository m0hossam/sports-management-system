using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class HostRequest
    {
        public int Id { get; set; }

        public bool IsApproved { get; set; }

        public ClubRepresentative? ClubRepresentative { get; set;}

        public Match? Match { get; set; }

        public Stadium? Stadium { get; set; }
    }
}
