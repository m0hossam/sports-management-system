﻿using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public Fan? Fan { get; set; }

        [Required]
        public Match? Match { get; set; }
    }
}
