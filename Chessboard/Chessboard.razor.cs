using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChessProject.App.Chessboard
{
	public partial class Chessboard : ComponentBase
	{
		[Inject] IJSRuntime? JSRuntime { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
			var orientation = "white";
			if (JSRuntime is not null)
			{
				await JSRuntime.InvokeVoidAsync("renderBoard", fen, orientation, orientation);
			}
		}
	}
}
