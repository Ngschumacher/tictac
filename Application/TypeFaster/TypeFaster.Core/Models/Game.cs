using System.Collections.Generic;
using TypeFaster.Models;

namespace TypeFaster.Core.Models
{
    public class Game
    {
        public int Id { get; set; }
        public ICollection<Move> Moves { get; set; } = new List<Move>();
        public User Player1 { get; set; }
        public int Player1Id { get; set; }
        
        public User Player2 { get; set; }
        public int Player2Id { get; set; }

        public int CurrentTurn { get; set; }
    }

    public class GameInformationViewModel
    {
        public Game Game { get; set; }
        public Board Board { get; set; }
        public GameStatus GameStatus { get; set; }
        
    }

    public class GameStatus
    {
        public bool GameEnded { get; set; }
        public User Winner { get; set; }
    }
}