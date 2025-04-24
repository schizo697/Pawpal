using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Time> Times { get; set; }

    }
}
