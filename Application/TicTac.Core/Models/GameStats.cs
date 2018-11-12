namespace TicTac.Core.Models {
    public class GameStats {
        public int Id { get; set; }
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public User Winner { get; set; }
        public bool Ended { get; set; }
    }
}