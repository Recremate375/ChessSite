using Microsoft.Identity.Client;

namespace ChessSite.Models.ModelsForDB
{
    public class Person
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int BlitzRate { get; set; }
        public int RapidRate { get; set; }
        public int BulletRate { get; set; }
        public int ClassicRate { get; set; }
    }
}
