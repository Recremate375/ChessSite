using ChessProject.Domain.Models;

namespace ChessProject.Application.Lobby;

public struct PairingPreference
{
    public SidePreference SidePreference { get; set; }
    public int TimeControle { get; set; }
    public int Increment { get; set; }
}