using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChessProject.App.Chessgame;

public partial class Chessgame : ComponentBase
{
    [Inject] private IJSRuntime? JsRuntime { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        var orientation = "white";

        if (JsRuntime is not null)
        {
            await JsRuntime.InvokeVoidAsync("renderBoard", fen, orientation, orientation);
        }
    }
}