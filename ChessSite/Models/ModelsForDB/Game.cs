namespace ChessSite.Models.ModelsForDB
{
    public class Game
    {
        public int Id { get; set; }
        public Person WhitePlayer { get; set; }
        public Person BlackPlayer { get; set; }
        public Result Result { get; set; }
        public string BlackTime { get; set; }
        public string WhiteTime { get; set; }
        public List<Notation> Notations { get; set; }
        public GameType Type { get; set; }
    }
}
