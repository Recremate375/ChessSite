using ChessProject.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChessProject.Infrastracture.Dal;

public class ChessProjectDbContext : IdentityDbContext
{
    public ChessProjectDbContext(DbContextOptions options) : base(options)
    { }
    
    public DbSet<ChessPlayer> Players { get; set; }
}
