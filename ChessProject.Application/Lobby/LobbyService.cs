using ChessProject.Domain.Models;
using ChessProject.Infrastracture.Dal;
using Timer = System.Timers.Timer;

namespace ChessProject.Application.Lobby;

public class LobbyService
{
    public event Action<ChessGame>? OnGameCreated;
    public Dictionary<Guid, ChessGame> OngoingGames { get; } = new();
    public Dictionary<ChessPlayer, PairingPreference> PlayersInOrder { get; set; } = new();


    public void RemovePlayerFromLobby(ChessPlayer player)
    {
        PlayersInOrder.Remove(player);
    }

    public void AddPlayerToLobby(ChessPlayer player, PairingPreference preference)
    {
        PlayersInOrder.Add(player, preference);
        var game = OngoingGames.Select(x => x.Value).Where(x =>
                x.TimeControl.Minutes == preference.TimeControle && x.TimeControl.Increment == preference.Increment)
            .ToList();
        if (game.Count == 0)
        {
            CreateGame(player, preference);
        }
        else
        {
            StartGame(player, game[0]);
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
            }
            else
            {
                game.WhiteChessPlayer = player;
            }
        }

        TimeControl timeControl = new TimeControl
        {
            Minutes = preference.TimeControle,
            Increment = preference.Increment,
            TimeControlId = 1
        };
        game.TimeControl = timeControl;
        game.RatingType = RatingType.Rated;
        OngoingGames.Add(game.UniqId, game);
        WaitingConnection(game);
    }

    public ChessGame checkGame(ChessPlayer player)
    {
        var waitingGame = OngoingGames.Select(x => x.Value).
            Where(x => x.WhiteChessPlayer == player || x.BlackChessPlayer == player).FirstOrDefault();
        OnGameCreated?.Invoke(waitingGame);
        return waitingGame;
    }
    public void WaitingConnection(ChessGame game)
    {
        //Timer timer = new Timer();
            while (game.WhiteChessPlayer != null && game.BlackChessPlayer != null)
            {
                var waitingGame = OngoingGames.Select(x => x.Value).Where(x => x.UniqId == game.UniqId).FirstOrDefault();
                if (waitingGame.WhiteChessPlayer != null && waitingGame.BlackChessPlayer != null)
                {
                    OnGameCreated?.Invoke(waitingGame);
                    break;
                }
                else
                {
                    waitingGame = null;
                }
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