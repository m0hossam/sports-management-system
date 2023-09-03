using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Models;

namespace SportsWebApp.Data
{
    public class SportsWebAppContext : DbContext
    {
        public SportsWebAppContext (DbContextOptions<SportsWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<SportsWebApp.Models.AssociationManager> AssociationManager { get; set; } = default!;

        public DbSet<SportsWebApp.Models.Club> Club { get; set; } = default!;

        public DbSet<SportsWebApp.Models.ClubRep> ClubRep { get; set; } = default!;

        public DbSet<SportsWebApp.Models.Fan> Fan { get; set; } = default!;

        public DbSet<SportsWebApp.Models.HostRequest> HostRequest { get; set; } = default!;

        public DbSet<SportsWebApp.Models.SportsMatch> SportsMatch { get; set; } = default!;

        public DbSet<SportsWebApp.Models.Stadium> Stadium { get; set; } = default!;

        public DbSet<SportsWebApp.Models.StadiumManager> StadiumManager { get; set; } = default!;

        public DbSet<SportsWebApp.Models.SystemAdmin> SystemAdmin { get; set; } = default!;
        
        public DbSet<SportsWebApp.Models.Ticket> Ticket { get; set; } = default!;

        public DbSet<SportsWebApp.Models.TicketTransaction> TicketTransaction { get; set; } = default!;

    }
}
