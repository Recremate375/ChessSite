using ChessProject.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChessProject.Infrastracture.Dal;

public class ChessProjectDbContext : IdentityDbContext
{
    public ChessProjectDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChessGame>().HasOne(g => g.BlackChessPlayer).WithMany()
            .HasForeignKey(g => g.BlackChessPlayerId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<ChessGame>().HasOne(g => g.WhiteChessPlayer).WithMany()
            .HasForeignKey(g => g.WhiteChessPlayerId).OnDelete(DeleteBehavior.NoAction);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<ChessPlayer> Players { get; set; }
    public DbSet<ChessGame> Games { get; set; }
    public DbSet<TimeControl> TimeControls { get; set; }
}
