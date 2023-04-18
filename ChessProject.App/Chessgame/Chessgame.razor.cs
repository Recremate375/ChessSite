using ChessProject.Application.Lobby;
using ChessProject.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    private bool Myturn;
    private bool Oppturn;

    public event EventHandler TimerUpdated;    

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
        _hubConnection.On<string, string>("PlayerMove", (move, fen) =>
        {
            Myturn = !Myturn;
            Oppturn = !Oppturn;
            Console.WriteLine($"Move: {move}, {Myturn}");
            JsRuntime.InvokeVoidAsync("makeOpponentMove", move, Myturn);
            Fen = fen;
        });
    }
    
    private void checkColorSide()
    {
        if (Name == _chessgame.BlackChessPlayer.Login)
        {
            Orientation = "black";
            side = 'b';
            Myturn = false;
            Oppturn = true;
            OponentName = _chessgame.WhiteChessPlayer.Login;
        }
        else if(Name == _chessgame.WhiteChessPlayer.Login)
        {
            Orientation = "white";
            side = 'w';
            Myturn = true;
            Oppturn = false;
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
        var _objRef = DotNetObjectReference.Create(this);
        if (JsRuntime is not null)
        {
            await JsRuntime.InvokeAsync<string>("SetDotNetHelper", _objRef);
            await JsRuntime.InvokeVoidAsync("renderBoard", Fen, Orientation, side);
            
        }
    }

    [JSInvokable]
    public void AuthenticatedPlayerMoved(string _lastMove, string fen)
    {
        Fen = fen;
        sendMove(_lastMove);
    }

    private async Task sendMove(string move)
    {
        await _hubConnection.InvokeAsync("SendMove", move, Fen);
    }

    /*private async Task DecrementTime()
    {
        int myMinutes = Int32.Parse(Myminutes);
        int mySeeconds = Int32.Parse(Myseconds);
        int OppMinutes = Int32.Parse(Oponentminutes);
        int OppSeeconds = Int32.Parse(Oponentseconds);
        if (turn == "white")
        {
            if (side == 'b')
            {
                while (OppMinutes > 0 || OppSeeconds > 0)
                {
                    if (OppMinutes == 0 && OppSeeconds == 0)
                    {
                        _chessgame.Result = ChessGameResult.BlackWin;
                        await JsRuntime.InvokeVoidAsync("timeLose");
                    }

                    if (OppMinutes == 0)
                    {
                        OppMinutes--;
                        OppSeeconds = 59;
                        if (OppMinutes < 10)
                        {
                            Myminutes = "0" + OppMinutes.ToString();
                        }
                        else
                        {
                            Myminutes = OppMinutes.ToString();
                        }

                        Myseconds = OppSeeconds.ToString();
                    }
                    else
                    {
                        OppSeeconds--;
                        Myseconds = OppSeeconds.ToString();
                    }

                    await Task.Delay(1000);
                
                }
            }
            while (myMinutes > 0 || mySeeconds > 0)
            {
                if (myMinutes == 0 && mySeeconds == 0)
                {
                    _chessgame.Result = ChessGameResult.BlackWin;
                    await JsRuntime.InvokeVoidAsync("timeLose");
                }

                if (mySeeconds == 0)
                {
                    myMinutes--;
                    mySeeconds = 59;
                    if (myMinutes < 10)
                    {
                        Myminutes = "0" + myMinutes.ToString();
                    }
                    else
                    {
                        Myminutes = myMinutes.ToString();
                    }

                    Myseconds = mySeeconds.ToString();
                }
                else
                {
                    mySeeconds--;
                    Myseconds = mySeeconds.ToString();
                }

                await Task.Delay(1000);
                
            }
        }
        else if(turn == "black")
        {
            if (side == 'w')
            {
                while (OppMinutes > 0 || OppSeeconds > 0)
                {
                    if (OppMinutes == 0 && OppSeeconds == 0)
                    {
                        _chessgame.Result = ChessGameResult.BlackWin;
                        await JsRuntime.InvokeVoidAsync("timeLose");
                    }

                    if (OppMinutes == 0)
                    {
                        OppMinutes--;
                        OppSeeconds = 59;
                        if (OppMinutes < 10)
                        {
                            Myminutes = "0" + OppMinutes.ToString();
                        }
                        else
                        {
                            Myminutes = OppMinutes.ToString();
                        }

                        Myseconds = OppSeeconds.ToString();
                    }
                    else
                    {
                        OppSeeconds--;
                        Myseconds = OppSeeconds.ToString();
                    }

                    await Task.Delay(1000);
                
                }
            }
            while (myMinutes > 0 || mySeeconds > 0)
            {
                if (myMinutes == 0 && mySeeconds == 0)
                {
                    _chessgame.Result = ChessGameResult.WhiteWin;
                    await JsRuntime.InvokeVoidAsync("timeLose");
                }

                if (mySeeconds == 0)
                {
                    myMinutes--;
                    mySeeconds = 59;
                }
                else
                {
                    mySeeconds--;
                }

                await Task.Delay(1000);
                
            }
        }
    }*/

}
