using ChessProject.Application.Lobby;
using ChessProject.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace ChessProject.App.Chessgame;

public partial class Chessgame : ComponentBase
{
    [Inject] private LobbyService lobby { get; set; }
    [Inject] private IJSRuntime? JsRuntime { get; set; }
    [Inject] protected AuthenticationStateProvider AuthState { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public string? Text { get; set; }
    [Parameter] public Domain.Models.ChessGame _chessgame { get; set; }
    private string Name { get; set; } 
    private string OponentName { get; set; }
    private string? Myminutes { get; set; } 
    private string? Myseconds { get; set; } 
    private string? Oponentminutes { get; set; } 
    private string? Oponentseconds { get; set; } 
    private int Increment { get; set; }
    private string? Fen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private string? Orientation { get; set; } 
    private char? side { get; set; } = 'w';
    private static string? lastMove;
    [Parameter] public string LastMove
    {
        get { return lastMove;}
        set { lastMove = value; }
        
    }
    private string? LastBlackMove { get; set; }
    private HubConnection _hubConnection;
    
    protected override async Task OnInitializedAsync()
    {
        var auth = AuthState;
        var authTask = auth.GetAuthenticationStateAsync().Result;
        if (authTask.User.Identity != null && !authTask.User.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/Identity/Account/Login", true);
        }
        
        Name = authTask?.User?.Identity?.Name;
        _chessgame = lobby.GetGameById(Guid.Parse(Text));
        checkColorSide();
        setTime();
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/gamehub")).Build();

        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            await _hubConnection.StartAsync();
        }
        await _hubConnection.InvokeAsync("JoinGame", Name);
    }
    
    private void checkColorSide()
    {
        if (Name == _chessgame.BlackChessPlayer.Login)
        {
            Orientation = "black";
            side = 'b';
            OponentName = _chessgame.WhiteChessPlayer.Login;
        }
        else if(Name == _chessgame.WhiteChessPlayer.Login)
        {
            Orientation = "white";
            side = 'w';
            OponentName = _chessgame.BlackChessPlayer.Login;
        }
    }

    private void setTime()
    {
        if (_chessgame.Variant == ChessGameVariant.Rapid || _chessgame.Variant == ChessGameVariant.Classical)
        {
            Myminutes = Oponentminutes = _chessgame.TimeControl.Minutes.ToString();
        }
        else
        {
            Myminutes = Oponentminutes = "0" + _chessgame.TimeControl.Minutes.ToString();
        }
        Myseconds = Oponentseconds = "00";
        Increment = _chessgame.TimeControl.Increment;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (JsRuntime is not null)
        {
            await JsRuntime.InvokeVoidAsync("renderBoard", Fen, Orientation, side);
        }
    }

    [JSInvokable]
    public static void AuthenticatedPlayerMoved(string _lastMove)
    {
        lastMove = _lastMove;
        sendMove(lastMove);
    }

    public async Task sendMove(string move)
    {
        await _hubConnection.InvokeAsync("SendMove", move);
        Console.WriteLine(move);
    }

    public async Task PlayerMove(string move)
    {
        await JsRuntime.InvokeVoidAsync("makeOpponentMove", move);
    }
}