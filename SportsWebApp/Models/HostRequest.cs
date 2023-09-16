﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWebApp.Models
{
    public class HostRequest
    {
        public int Id { get; set; }

        public bool IsApproved { get; set; }

        [Required]
        public ClubRepresentative? ClubRepresentative { get; set;}

        [Required]
        public Match? Match { get; set; }

        [Required]
        public Stadium? Stadium { get; set; }
    }
}
