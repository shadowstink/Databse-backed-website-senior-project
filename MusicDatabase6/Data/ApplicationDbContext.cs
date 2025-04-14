using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicDatabase6.Models;

namespace MusicDatabase6.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<Musician> Musician { get; set; }
        public DbSet<Song> Song { get; set; }
    }
}
