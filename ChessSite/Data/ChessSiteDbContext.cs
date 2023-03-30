using ChessSite.Models.ModelsForDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ChessSite.Data
{
    public class ChessSiteDbContext : DbContext
    {
        public ChessSiteDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Notation> Notations { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}
