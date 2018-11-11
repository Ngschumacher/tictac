using System.Collections.Generic;
using TypeFaster.Core.Models;

namespace TypeFaster.Core.Interfaces
{
    public interface IGameService
    {
        GameModel NewGame(int player1, int player2);
        GameModel MakeMove(int gameId, int playerId, int move);
        GameModel GetGame(int id);
        List<GameStats> GetStatsVersusOpponent(int userId, int opponentId);
    }
}