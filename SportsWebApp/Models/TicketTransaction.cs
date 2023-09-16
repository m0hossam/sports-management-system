using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SportsWebApp.Models
{
    public class TicketTransaction
    {
        public int Id { get; set; }

        public Ticket? Ticket { get; set; }

        public Match? Match { get; set; }

        public Fan? Fan { get; set; }

    }
}
