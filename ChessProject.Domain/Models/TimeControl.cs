namespace ChessProject.Domain.Models;

public class TimeControl
{
    public int TimeControlId { get; set; }
    public int Minutes { get; set; }
    public int Increment { get; set; }

    public override string ToString()
    {
        return $"{Minutes} + {Increment}";
    }

    public ChessGameVariant GetVariantFromTimeControl()
    {
        return Minutes switch
        {
            <= 2 => ChessGameVariant.Bullet,
            <= 9 => ChessGameVariant.Blitz,
            <= 15 => ChessGameVariant.Rapid,
            _ => ChessGameVariant.Classical
        };
    }
}