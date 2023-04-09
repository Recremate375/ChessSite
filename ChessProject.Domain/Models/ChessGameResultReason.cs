namespace ChessProject.Domain.Models;

public enum ChessGameResultReason
{
    //Game result reason (time is out, checkmate, or something else)
    Checkmate,
    Resignation, 
    TimeElapsed, 
    Repetition,
    InsufficientMaterial, 
    DrawByAgreement, 
    LoseByAgreement
}