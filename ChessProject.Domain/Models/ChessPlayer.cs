namespace ChessProject.Domain.Models;

public class ChessPlayer
{
    public int ChessPlayerId { get; set; }
    public string? IdentityId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public List<ChessGame>? WhiteGames { get; set; } = new List<ChessGame>();
    public List<ChessGame>? BlackGames { get; set; } = new List<ChessGame>();
    public int BlitzRating { get; set; }
    public int BulletRating { get; set; }
    public int RapidRating { get; set; }
    public int ClassicalRating { get; set; }
}
