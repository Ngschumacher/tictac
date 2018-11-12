namespace TicTac.Core.Models {
    public class GameModel {
        public int Id { get; set; }
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public User WhosNext { get; set; }
        public GameStatus GameStatus { get; set; }
        public Board Board { get; set; }
    }
}