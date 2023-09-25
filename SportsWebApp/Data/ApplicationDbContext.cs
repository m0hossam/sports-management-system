using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsWebApp.Models;

namespace SportsWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<AssociationManager> AssociationManagers { get; set; }
        public DbSet<ClubRepresentative> ClubRepresentatives { get; set; }
        public DbSet<StadiumManager> StadiumManagers { get; set; }
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<HostRequest> HostRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}