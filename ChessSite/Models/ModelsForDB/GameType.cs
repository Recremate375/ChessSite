namespace ChessSite.Models.ModelsForDB
{
    public class GameType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string MinTime { get; set; }
        public string MaxTime { get; set; }
    }
}
