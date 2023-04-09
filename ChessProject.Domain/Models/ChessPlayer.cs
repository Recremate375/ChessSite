namespace ChessProject.Domain.Models;

public class ChessPlayer
{
    public int ChessPlayerId { get; set; }
    public string? IdentityId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public int BlitzRating { get; set; }
    public int BulletRating { get; set; }
    public int RapidRating { get; set; }
    public int ClassicalRating { get; set; }
}
