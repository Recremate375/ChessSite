using System.Diagnostics.CodeAnalysis;
using ChessProject.Domain.Models;
using ChessProject.Infrastracture.Dal;
using Timer = System.Timers.Timer;

namespace ChessProject.Application.Lobby;

public class LobbyService
{
    public event Action<ChessGame>? OnGameCreated;
    public Dictionary<Guid, ChessGame> OngoingGames { get; set; }
    public Dictionary<ChessPlayer, PairingPreference> PlayersInOrder { get; set; } = new();


    public void RemovePlayerFromLobby(ChessPlayer player)
    {
        PlayersInOrder.Remove(player);
    }

    public void AddPlayerToLobby(ChessPlayer player, PairingPreference preference)
    {
        PlayersInOrder.Add(player, preference);
        ChessGame? game = new ChessGame();
        if (OngoingGames is not null)
        {
            game = OngoingGames.FirstOrDefault(
                x => x.Value.TimeControl.Minutes == preference.TimeControle
                     && x.Value.TimeControl.Increment == preference.Increment).Value;
        }

        if ((game.WhiteChessPlayer == null && game.BlackChessPlayer == null) || game == null)
        {
            CreateGame(player, preference);
        }
        else
        {
            StartGame(player, game);
        }
    }

    private void CreateGame(ChessPlayer player, PairingPreference preference)
    {
        ChessGame game = new ChessGame();
        Guid gameId = Guid.NewGuid();
        game.UniqId = gameId;
        if (preference.SidePreference == SidePreference.Random)
        {
            var random = new Random().Next(0, 1);
            if (random == 0)
            {
                game.BlackChessPlayer = player;
                game.BlackChessPlayerId = player.ChessPlayerId;
            }
            else
            {
                game.WhiteChessPlayer = player;
                game.WhiteChessPlayerId = player.ChessPlayerId;
            }
        }
        
        TimeControl timeControl = new TimeControl
        {
            Minutes = preference.TimeControle,
            Increment = preference.Increment
        };
        game.TimeControl = timeControl;
        game.RatingType = RatingType.Rated;
        if (OngoingGames == null)
        {
            OngoingGames = new();
        }
        OngoingGames.Add(game.UniqId, game);
        WaitingConnection(game);
    }

    public ChessGame GetGameById(Guid id)
    {
        return OngoingGames[id];
    }
    public ChessGame СheckGame(ChessPlayer player)
    {
        ChessGame? waitingGame = OngoingGames.FirstOrDefault(
            x => x.Value.BlackChessPlayer == player ||
                 x.Value.WhiteChessPlayer == player).Value;
        OnGameCreated?.Invoke(waitingGame);
        return waitingGame;
    }

    public void WaitingConnection(ChessGame game)
    {
        ChessGame? waitingGame = game;
        while(true)
        {
            if (waitingGame != null)
            {
                if (waitingGame.WhiteChessPlayer != null && waitingGame.BlackChessPlayer != null)
                {
                    OnGameCreated?.Invoke(waitingGame);
                    break; 
                }
            }
            
            waitingGame = null;
            waitingGame = OngoingGames.FirstOrDefault(x => x.Key == game.UniqId).Value;
        }
    }
    private void StartGame(ChessPlayer player, ChessGame game)
    {
        if (game.BlackChessPlayer == null)
        {
            game.BlackChessPlayer = player;
        }
        else if (game.WhiteChessPlayer == null)
        {
            game.WhiteChessPlayer = player;
        }
        OngoingGames[game.UniqId] = game;
    }
}