using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Data
{
    public class TestAppContext : DbContext
    {
        public TestAppContext (DbContextOptions<TestAppContext> options)
            : base(options)
        {
        }

        public DbSet<TestApp.Models.Dummy> Dummy { get; set; } = default!;
    }
}
