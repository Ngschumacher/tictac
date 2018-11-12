using System.Collections.Generic;
using TicTac.Core.Models;

namespace TicTac.Core.Interfaces {
    public interface IGameRepository {
        Game CreateGame(int player1Id, int player2Id);
        Game GetGame(int id);
        void SaveGame(Game game);
        List<Game> GetGamesVersusOpponenet(int userId, int opponentId);
    }
}