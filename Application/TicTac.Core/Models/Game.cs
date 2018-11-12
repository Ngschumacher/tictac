using System.Collections.Generic;

namespace TicTac.Core.Models {
    public class Game {
        public int Id { get; set; }
        public ICollection<Move> Moves { get; set; } = new List<Move>();
        public User Player1 { get; set; }
        public int Player1Id { get; set; }

        public User Player2 { get; set; }
        public int Player2Id { get; set; }

        public int StartingPlayerId { get; set; }
    }
}