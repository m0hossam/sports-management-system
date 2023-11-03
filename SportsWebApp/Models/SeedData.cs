using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportsWebApp.Data;
using System;
using System.Linq;

namespace SportsWebApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any Clubs.
            if (context.Clubs.Any() || context.Stadiums.Any() || context.Matches.Any())
            {
                return;   // DB has been seeded
            }
            context.Clubs.AddRange(
                new Club
                {
                    Name = "Liverpool",
                    Location = "Liverpool, UK"
                },
                new Club
                {
                    Name = "Juventus",
                    Location = "Turin, Italy"
                },
                new Club
                {
                    Name = "Real Madrid",
                    Location = "Madrid, Spain"
                },
                new Club
                {
                    Name = "Bayern Munich",
                    Location = "Munich, Germany"
                },
                new Club
                {
                    Name = "Paris Saint-Germain",
                    Location = "Paris, France"
                }
            );
            context.Stadiums.AddRange(
                new Stadium
                {
                    Name = "Anfield Stadium",
                    Location = "Liverpool, UK",
                    Capacity = 53000
                },
                new Stadium
                {
                    Name = "Juventus Stadium",
                    Location = "Turin, Italy",
                    Capacity = 41000
                },
                new Stadium
                {
                    Name = "Santiago Bernabeu",
                    Location = "Madrid, Spain",
                    Capacity = 83000
                },
                new Stadium
                {
                    Name = "Allianz Arena",
                    Location = "Munich, Germany",
                    Capacity = 75000
                },
                new Stadium
                {
                    Name = "Parc des Princes",
                    Location = "Paris, France",
                    Capacity = 47000
                }
            );

            context.SaveChanges();

            context.Matches.AddRange(
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Liverpool"),
                    AwayClub = context.Clubs.First(x => x.Name == "Juventus"),
                    StartTime = DateTime.Parse("2024-6-5 12:00:00"),
                    EndTime = DateTime.Parse("2024-6-5 14:00:00")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Bayern Munich"),
                    AwayClub = context.Clubs.First(x => x.Name == "Real Madrid"),
                    StartTime = DateTime.Parse("2023-12-27 15:00:00"),
                    EndTime = DateTime.Parse("2023-12-27 17:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Allianz Arena"),
                    NumberOfAttendees = 12056
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Real Madrid"),
                    AwayClub = context.Clubs.First(x => x.Name == "Paris Saint-Germain"),
                    StartTime = DateTime.Parse("2024-1-17 17:00:00"),
                    EndTime = DateTime.Parse("2024-1-17 19:00:00")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Juventus"),
                    AwayClub = context.Clubs.First(x => x.Name == "Bayern Munich"),
                    StartTime = DateTime.Parse("2023-10-12 21:00:00"),
                    EndTime = DateTime.Parse("2023-10-12 23:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Juventus Stadium"),
                    NumberOfAttendees = 32628
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Paris Saint-Germain"),
                    AwayClub = context.Clubs.First(x => x.Name == "Liverpool"),
                    StartTime = DateTime.Parse("2023-11-3 20:00:00"),
                    EndTime = DateTime.Parse("2023-11-3 22:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Parc des Princes"),
                    NumberOfAttendees = 29809
                }
            ); ;

            context.SaveChanges();
        }
    }
}