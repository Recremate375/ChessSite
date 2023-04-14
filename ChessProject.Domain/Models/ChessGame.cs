using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessProject.Domain.Models;

public class ChessGame
{
    public int ChessGameId { get; set; }
    public Guid UniqId { get; set; }
    public string? PGN { get; set; }
    public int? WhiteChessPlayerId { get; set; }
    public int? BlackChessPlayerId { get; set; }
    public ChessPlayer? WhiteChessPlayer { get; set; }
    public ChessPlayer? BlackChessPlayer { get; set; }
    public ChessGameResult? Result { get; set; }
    public ChessGameResultReason? ResultReason { get; set; }
    public DateTime? DatePlayed { get; set; }
    public TimeControl TimeControl { get; set; }
    public ChessGameVariant Variant => TimeControl.GetVariantFromTimeControl();
    public RatingType RatingType { get; set; }
}