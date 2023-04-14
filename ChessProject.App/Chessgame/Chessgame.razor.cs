using ChessProject.App.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ChessProject.App.Chessgame;

public partial class Chessgame : ComponentBase
{
    [Inject] private IJSRuntime? JsRuntime { get; set; }
    [Inject] protected AuthenticationStateProvider AuthState { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public Domain.Models.ChessGame _chessgame { get; set; } 
    [Parameter] public GameHub hub { get; set; }
    [Parameter] public string Name { get; set; }
    [Parameter] public string OponentName { get; set; }
    private string? Fen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private string? Orientation { get; set; } = "white";
    private string? LastWhiteMove { get; set; }
    private string? LastBlackMove { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var auth = AuthState;
        var authTask = auth.GetAuthenticationStateAsync().Result;
        if (authTask.User.Identity != null && !authTask.User.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/Identity/Account/Login", true);
        }

        Name = authTask.User.Identity.Name;
        
        //hub.OnConnectedAsync();
        

    }

    private void checkColorSide()
    {
        
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (JsRuntime is not null)
        {
            await JsRuntime.InvokeVoidAsync("renderBoard", Fen, Orientation, 'w');
        }
    }
    [JSInvokable]
    public async Task AuthenticatedPlayerMoved(string? lastMove)
    {
        
        LastWhiteMove = lastMove;
    }
}