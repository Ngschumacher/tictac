using TypeFaster.Core.Models;
using TypeFaster.Models;

namespace TypeFaster.Business.Interfaces
{
    public interface IBoardService
    {
        Game NewGame(int player1, int player2);
        Board MakeMove(int gameId, int playerId, int move);
        Game GetGame(int id);
    }
}