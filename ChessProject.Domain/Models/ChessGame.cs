namespace ChessProject.Domain.Models;

public class ChessGame
{
    public Guid GameId { get; set; }
    public string? PGN { get; set; }
    public int WhiteChessPlayerId { get; set; }
    public ChessPlayer? WhiteChessPlayer { get; set; }
    public int BlackChessPlayerId { get; set; }
    public ChessPlayer? BlackChessPlayer { get; set; }
    public ChessGameResult? Result { get; set; }
    public ChessGameResultReason? ResultReason { get; set; }
    public DateTime? DatePlayed { get; set; }
    public TimeControl TimeControl { get; set; }
    public ChessGameVariant Variant => TimeControl.GetVariantFromTimeControl();
    public RatingType RatingType { get; set; }
}