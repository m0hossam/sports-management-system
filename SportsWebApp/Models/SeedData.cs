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
                    Name = "Arsenal",
                    Location = "London, UK"
                },
                new Club
                {
                    Name = "Roma",
                    Location = "Rome, Italy"
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
                },
                new Club
                {
                    Name = "Al Ahly",
                    Location = "Cairo, Egypt"
                }
            );
            context.Stadiums.AddRange(
                new Stadium
                {
                    Name = "Emirates Stadium",
                    Location = "London, UK",
                    Capacity = 60000
                },
                new Stadium
                {
                    Name = "Stadio Olimpico",
                    Location = "Rome, Italy",
                    Capacity = 70000
                },
                new Stadium
                {
                    Name = "Santiago Bernabeu",
                    Location = "Mardid, Spain",
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
                },
                new Stadium
                {
                    Name = "Al Salam Stadium",
                    Location = "Cairo, Egypt",
                    Capacity = 30000
                }
            );

            context.SaveChanges();

            context.Matches.AddRange(
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Arsenal"),
                    AwayClub = context.Clubs.First(x => x.Name == "Real Madrid"),
                    StartTime = DateTime.Parse("2023-12-12 12:00:00"),
                    EndTime = DateTime.Parse("2023-12-12 14:00:00")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Bayern Munich"),
                    AwayClub = context.Clubs.First(x => x.Name == "Roma"),
                    StartTime = DateTime.Parse("2023-12-12 15:00:00"),
                    EndTime = DateTime.Parse("2023-12-12 17:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Allianz Arena")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Real Madrid"),
                    AwayClub = context.Clubs.First(x => x.Name == "Arsenal"),
                    StartTime = DateTime.Parse("2023-12-12 7:00:00"),
                    EndTime = DateTime.Parse("2023-12-12 9:00:00")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Roma"),
                    AwayClub = context.Clubs.First(x => x.Name == "Bayern Munich"),
                    StartTime = DateTime.Parse("2020-12-12 1:00:00"),
                    EndTime = DateTime.Parse("2020-12-12 3:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Stadio Olimpico")
                },
                new Match
                {
                    HomeClub = context.Clubs.First(x => x.Name == "Paris Saint-Germain"),
                    AwayClub = context.Clubs.First(x => x.Name == "Al Ahly"),
                    StartTime = DateTime.Parse("2022-12-12 9:00:00"),
                    EndTime = DateTime.Parse("2023-12-12 9:00:00"),
                    Stadium = context.Stadiums.First(x => x.Name == "Parc des Princes")
                }
            ); ;

            context.SaveChanges();
        }
    }
}