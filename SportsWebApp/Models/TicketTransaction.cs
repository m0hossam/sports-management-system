using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class TicketTransaction
    {
        public int Id { get; set; }

        [Required]
        public Ticket? Ticket { get; set; }

        [Required]
        public Match? Match { get; set; }

        [Required]
        public Fan? Fan { get; set; }

    }
}
