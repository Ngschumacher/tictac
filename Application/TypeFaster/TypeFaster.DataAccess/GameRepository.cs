using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TypeFaster.Core.Models;
using TypeFaster.DataAccess.Contexts;

namespace TypeFaster.DataAccess
{
    public class GameRepository : IGameRepository {
        private readonly GameContext _gameContext;

        public GameRepository(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public Game CreateGame(int player1Id, int player2Id)
        {
                
            var  player1 = _gameContext.User.FirstOrDefault(x => x.Id == player1Id);
            if (player1 == null)
                throw new Exception($"Player one with {player1Id} not found");
            
            var player2 = _gameContext.User.FirstOrDefault(x => x.Id == player2Id);
            if (player2 == null)
                throw new Exception($"Player two with {player2Id} not found ");
            
                Random rnd = new Random();
                var game = new Game()
                {
                    Player1Id = player1.Id,
                    Player2Id = player2.Id,
                    StartingPlayerId = (rnd.Next(0, 1) == 0) ? player1.Id : player2.Id,
                };
                var testGame = _gameContext.Game.Add(game);

                _gameContext.SaveChanges();

                game.Player1 = player1;
                game.Player2 = player2;


            return game;
        }
        
        public Game GetGame(int id)
        {
                var game =  _gameContext.Game
                    .Include(g => g.Player1)
                    .Include(g => g.Player2)
                    .FirstOrDefault(x => x.Id == id);

            if (game == null)
                return null;
            
            var player1 = _gameContext.User.FirstOrDefault(x => x.Id == game.Player1Id);
            game.Player1 = player1;

            var player2 = _gameContext.User.FirstOrDefault(x => x.Id == game.Player2Id);
            game.Player2 = player2;
            
            var moves = _gameContext.Move.Where(m => m.GameId == game.Id).ToList();
            game.Moves = moves;
            
            
            return game;
        }

        public void SaveGame(Game game)
        {
            _gameContext.Game.Update(game);
            _gameContext.SaveChanges();
        }

        public List<Game> GetGamesVersusOpponenet(int userId, int opponentId)
        {
            return _gameContext.Game.Where(g =>
                g.Player1Id == userId || g.Player1Id == opponentId
                &&
                g.Player2Id == userId || g.Player2Id == opponentId
            )
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.Moves)
                .OrderByDescending(g => g.Id)
                .ToList();
        }
    }

    public interface IGameRepository
    {
        Game CreateGame(int player1Id, int player2Id);
        Game GetGame(int id);
        void SaveGame(Game game);
        List<Game> GetGamesVersusOpponenet(int userId, int opponentId);
    }
}