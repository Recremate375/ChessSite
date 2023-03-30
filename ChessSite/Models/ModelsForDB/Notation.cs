using Microsoft.Identity.Client;

namespace ChessSite.Models.ModelsForDB
{
    public class Notation
    {
        public int Id { get; set; }
        public Game GameId {  get; set; }
        public string PlayerName { get; set; }
        public string MoveTo { get; set; }
        public string MoveFrom { get; set; }
        public string FigureType { get; set; }
        public string MoveTime { get; set; }
        public bool IsCapture { get; set; }
        public bool IsEndOfGame { get; set; }
    }
}
