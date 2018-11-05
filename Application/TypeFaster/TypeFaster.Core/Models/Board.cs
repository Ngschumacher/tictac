namespace TypeFaster.Core.Models
{
    public class Board
    {
        public char[] Positions { get; set; } = new char[9]
        {
            ' ', ' ', ' ',
            ' ', ' ', ' ',
            ' ', ' ', ' '
        };

        public class MakeMoveReponse
        {
            public bool GameIsOngoing { get; set; }
            public Board Board { get; set; }
        }
    }
}